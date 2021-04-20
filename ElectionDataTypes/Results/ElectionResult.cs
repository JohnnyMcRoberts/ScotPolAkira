namespace ElectionDataTypes.Results
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using Interfaces;

    public class ElectionResult
    {
        #region Private Data

        private IPartiesProvider _parties;
        private IConstituencyResultProvider _firstVotes;
        private IConstituencyResultProvider _secondVotes;

        #endregion region

        #region Public Properties

        public int Year { get; set; }
        public IPartiesProvider Parties
        {
            get => _parties;
            set
            {
                _parties = value;
                CalculateResults();
            }
        }
        public IConstituencyResultProvider FirstVotes
        {
            get => _firstVotes;
            set
            {
                _firstVotes = value;
                CalculateResults();
            }
        }
        public IConstituencyResultProvider SecondVotes
        {
            get => _secondVotes;
            set
            {
                _secondVotes = value;
                CalculateResults();
            }
        }

        public List<RegionResult> RegionResults { get; }
        public List<PartyVote> PartyVotes { get; private set; }

        #endregion region

        #region Local Methods

        private List<PartyVote> CalculatePartyVotes()
        {
            Dictionary<string, PartyVote> partyVotesByAbbreviation =
                new Dictionary<string, PartyVote>();

            // Get the party constituency votes.
            CalculatePartyConstituencyVotes(partyVotesByAbbreviation);

            // Get the party list votes.
            CalculatePartyListVotes(partyVotesByAbbreviation);

            // Get the seats for each party from each region.
            CalculatePartySeats(partyVotesByAbbreviation);

            // Get the full names.
            foreach (string abbreviation in partyVotesByAbbreviation.Keys)
            {
                if (Parties.PartiesByAbbreviation.ContainsKey(abbreviation))
                {
                    partyVotesByAbbreviation[abbreviation].FullName =
                        Parties.PartiesByAbbreviation[abbreviation];
                }
            }

            // Sort them by total seats and list vote.
            List<PartyVote> sortedParties = 
                partyVotesByAbbreviation
                    .Values
                    .OrderByDescending(x => x.TotalSeats)
                    .ThenByDescending(x => x.TotalListVote)
                    .ToList();

            return sortedParties;
        }

        private void CalculatePartySeats(Dictionary<string, PartyVote> partyVotesByAbbreviation)
        {
            // Get the seats for each party from each region.
            foreach (RegionResult region in RegionResults)
            {
                // Set the constituency seats.
                foreach (string party in region.FirstVoteSeatsByParty.Keys)
                {
                    partyVotesByAbbreviation[party].ConstituencySeats +=
                        region.FirstVoteSeatsByParty[party].Count;
                }

                // Set the list seats.
                foreach (string party in region.AdditionalMembersByParty.Keys)
                {
                    partyVotesByAbbreviation[party].ListSeats +=
                        region.AdditionalMembersByParty[party];
                }
            }
        }

        private void CalculatePartyListVotes(Dictionary<string, PartyVote> partyVotesByAbbreviation)
        {
            // Get the party list votes.
            int totalListVotesCast = 0;
            foreach (ConstituencyResult secondVoteConstituency in SecondVotes.ResultsByName.Values)
            {
                totalListVotesCast += secondVoteConstituency.TotalValidVotesCast;

                foreach (PartyResult partyResult in secondVoteConstituency.PartyResults)
                {
                    if (partyVotesByAbbreviation.ContainsKey(partyResult.PartyAbbreviation))
                    {
                        partyVotesByAbbreviation[partyResult.PartyAbbreviation].TotalListVote
                            += partyResult.Votes;
                    }
                    else
                    {
                        PartyVote partyVote = new PartyVote
                        {
                            Abbreviation = partyResult.PartyAbbreviation,
                            TotalListVote = partyResult.Votes
                        };

                        partyVotesByAbbreviation.Add(partyResult.PartyAbbreviation, partyVote);
                    }
                }
            }

            // Calculate the list percentages.
            float listMultiplier = 100f / totalListVotesCast;
            foreach (string party in partyVotesByAbbreviation.Keys)
            {
                partyVotesByAbbreviation[party].PercentageListVote =
                    partyVotesByAbbreviation[party].TotalListVote * listMultiplier;
            }
        }

        private void CalculatePartyConstituencyVotes(Dictionary<string, PartyVote> partyVotesByAbbreviation)
        {
            // Get the party constituency votes.
            int totalConstituencyVotesCast = 0;
            foreach (ConstituencyResult firstVoteConstituency in FirstVotes.ResultsByName.Values)
            {
                totalConstituencyVotesCast += firstVoteConstituency.TotalValidVotesCast;

                foreach (PartyResult partyResult in firstVoteConstituency.PartyResults)
                {
                    if (partyVotesByAbbreviation.ContainsKey(partyResult.PartyAbbreviation))
                    {
                        partyVotesByAbbreviation[partyResult.PartyAbbreviation].TotalConstituencyVote
                            += partyResult.Votes;
                    }
                    else
                    {
                        PartyVote partyVote = new PartyVote
                        {
                            Abbreviation = partyResult.PartyAbbreviation,
                            TotalConstituencyVote = partyResult.Votes
                        };

                        partyVotesByAbbreviation.Add(partyResult.PartyAbbreviation, partyVote);
                    }
                }
            }

            // Calculate the constituency percentages.
            float constituencyMultiplier = 100f / totalConstituencyVotesCast;
            foreach (string party in partyVotesByAbbreviation.Keys)
            {
                partyVotesByAbbreviation[party].PercentageConstituencyVote =
                    partyVotesByAbbreviation[party].TotalConstituencyVote * constituencyMultiplier;
            }
        }

        #endregion region

        #region Public Methods

        public void CalculateResults()
        {
            if (Parties == null || FirstVotes == null || SecondVotes == null)
            {
                return;
            }

            List<string> regionNames =
                FirstVotes.ResultSetsByRegion.Keys.OrderBy(x => x).ToList();

            RegionResults.Clear();
            foreach (string regionName in regionNames)
            {
                if (SecondVotes.ResultSetsByRegion.ContainsKey(regionName))
                {
                    RegionResults.Add(
                        new RegionResult(
                            regionName,
                            FirstVotes.ResultSetsByRegion[regionName],
                            SecondVotes.ResultSetsByRegion[regionName]
                            ));
                }
            }

            PartyVotes = CalculatePartyVotes();

            Console.WriteLine("doing it");

        }

        #endregion region

        public ElectionResult()
        {
            Year = 2016;

            Parties = null;
            FirstVotes = null;
            SecondVotes = null;

            RegionResults = new List<RegionResult>();
        }
    }
}
