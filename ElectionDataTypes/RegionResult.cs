namespace ElectionDataTypes
{
    using System.Collections.Generic;
    using System.Linq;

    public class RegionResult : BaseResult
    {
        #region Constants

        /// <summary>
        /// The number of list seats for each region.
        /// </summary>
        private const int ListSeatsPerRegion = 7;

        #endregion

        #region Nested Classes

        public struct PartyQuota
        {
            public string Name;
            public int Votes;
            public int Round;
            public float Quotient;
        }

        #endregion

        #region Properties

        public List<ConstituencyResult> FirstVoteResults { get; }
        public List<ConstituencyResult> SecondVoteResults { get; }

        public Dictionary<string, List<ConstituencyResult>> FirstVoteSeatsByParty { get; set; }
        public Dictionary<string, PartyResult> SecondVoteByParty { get; set; }
        public Dictionary<string, int> AdditionalMembersByParty { get; set; }

        #endregion

        #region Local Utility Methods

        private void AddListConstituencyToTotals(ConstituencyResult constituency)
        {
            Electorate += constituency.Electorate;
            TotalBallotsAtTheCount += constituency.TotalBallotsAtTheCount;
            TotalValidVotesCast += constituency.TotalValidVotesCast;
            RejectedBallots += constituency.RejectedBallots;

            foreach (PartyResult partyResult in constituency.PartyResults)
            {
                if (SecondVoteByParty.ContainsKey(partyResult.PartyAbbreviation))
                {
                    SecondVoteByParty[partyResult.PartyAbbreviation].Votes +=
                        partyResult.Votes;
                }
                else
                {
                    SecondVoteByParty.Add(partyResult.PartyAbbreviation, partyResult);
                }
            }

            PartyResults = SecondVoteByParty.Values.ToList();
        }

        private void UpdatePercentages()
        {
            if (Electorate != 0)
            {
                float electoratePercentageMultiplier = 100.0f / Electorate;
                BallotBoxTurnoutPercentage = TotalValidVotesCast * electoratePercentageMultiplier;

                foreach (PartyResult partyResult in PartyResults)
                {
                    partyResult.PercentageOfVotes = partyResult.Votes * electoratePercentageMultiplier;
                }
            }
        }

        private void CalculateAdditionalMembers()
        {
            // Set up the seats by party to in initial constituency results.
            Dictionary<string, int> seatsByParty = new Dictionary<string, int>();
            foreach (string party in FirstVoteSeatsByParty.Keys)
            {
                seatsByParty.Add(party, FirstVoteSeatsByParty[party].Count);
            }

            // Allocate the list seats based on the D'Hondt method.
            AdditionalMembersByParty = new Dictionary<string, int>();
            for (int i = 0; i < ListSeatsPerRegion; i++)
            {
                List<PartyQuota> partyVoteQuotients = new List<PartyQuota>();
                foreach (PartyResult partyResult in PartyResults)
                {
                    string abbreviation = partyResult.PartyAbbreviation;
                    float divisor =
                        seatsByParty.ContainsKey(abbreviation) ? 1 + seatsByParty[abbreviation] : 1;
                    partyVoteQuotients.Add(
                        new PartyQuota
                        {
                            Name = abbreviation,
                            Votes = partyResult.Votes,
                            Round = i,
                            Quotient = partyResult.Votes / divisor
                        });
                }

                // Sort the results.
                partyVoteQuotients =
                    partyVoteQuotients.OrderByDescending(x => x.Quotient).ToList();

                // Assign the set to leading party on this round.
                PartyQuota roundWinner = partyVoteQuotients[0];

                if (AdditionalMembersByParty.ContainsKey(roundWinner.Name))
                {
                    AdditionalMembersByParty[roundWinner.Name]++;
                }
                else
                {
                    AdditionalMembersByParty.Add(roundWinner.Name, 1);
                }

                if (seatsByParty.ContainsKey(roundWinner.Name))
                {
                    seatsByParty[roundWinner.Name]++;
                }
                else
                {
                    seatsByParty.Add(roundWinner.Name, 1);
                }
            }
        }

        #endregion

        public RegionResult(
            string regionName,
            List<ConstituencyResult> firstVoteResults,
            List<ConstituencyResult> secondVoteResults)
        {
            Region = regionName;
            FirstVoteResults = firstVoteResults;
            SecondVoteResults = secondVoteResults;

            FirstVoteSeatsByParty = new Dictionary<string, List<ConstituencyResult>>();
            SecondVoteByParty = new Dictionary<string, PartyResult>();

            // Get the number of first vote seats already allocated.
            foreach (ConstituencyResult constituency in firstVoteResults)
            {
                if (FirstVoteSeatsByParty.ContainsKey(constituency.Win))
                {
                    FirstVoteSeatsByParty[constituency.Win].Add(constituency);
                }
                else
                {
                    FirstVoteSeatsByParty.Add(constituency.Win, new List<ConstituencyResult> { constituency });
                }
            }

            // Get the totals from the second votes.
            foreach (ConstituencyResult constituency in secondVoteResults)
            {
                AddListConstituencyToTotals(constituency);
            }

            // Update the percentages.
            UpdatePercentages();

            // Calculate the list seats
            CalculateAdditionalMembers();
        }

    }
}
