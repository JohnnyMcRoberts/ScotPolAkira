using ElectionDataTypes.Interfaces;
using ElectionDataTypes.Providers;

namespace ElectionDataTypes.Polling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Results;

    public class ElectionPrediction
    {
        #region Private Data

        #endregion region

        #region Public Properties

        public ElectionResult PreviousResult { get; set; }

        public OpinionPoll OpinionPoll { get; set; }

        public ElectionResult PredictedResult { get; set; }

        public List<PartyVoteSwing> PartyPredictions { get; set; }

        public DateTime PublishedDate => OpinionPoll.PublicationDate;
        public string Link => OpinionPoll.Link;
        public Uri LinkUri => OpinionPoll.LinkUri;
        public string PollingCompany => OpinionPoll.PollingCompany;

        #endregion region

        #region Local Methods

        #endregion region

        #region Public Methods

        private void SetupPartyPredictions()
        {
            // Copy over the year and notes to the predicted result.
            PredictedResult = new ElectionResult
            {
                Parties = PreviousResult.Parties,
                Year = PublishedDate.Year
            };

            // Get the party swings.
            SetupPartyVoteSwings();

            // Get the constituency swings.
            Dictionary<string, float> constituencySwingsByParty = 
                PartyPredictions.ToDictionary(
                    party => party.Abbreviation, 
                    party => party.PercentageConstituencyVoteSwing);

            // Copy over the constituency results from the previous and apply the swings.
            PredictedResult.FirstVotes =
                GetConstituencyResultProviderWithSwings(PreviousResult.FirstVotes, constituencySwingsByParty);

            // Get the list swings.
            Dictionary<string, float> listSwingsByParty =
                PartyPredictions.ToDictionary(
                    party => party.Abbreviation,
                    party => party.PercentageListVoteSwing);

            // Copy over the list results from the previous and apply the swings.
            PredictedResult.SecondVotes = 
                GetConstituencyResultProviderWithSwings(PreviousResult.SecondVotes, listSwingsByParty);

            // Update the parties seats.
            for (int i = 0; i < PartyPredictions.Count; i++)
            {
                string partyAbbreviation = PartyPredictions[i].Abbreviation;
                PartyVote predictedResultPartyVote =
                    PredictedResult.PartyVotes.FirstOrDefault(x => x.Abbreviation == partyAbbreviation);

                if (predictedResultPartyVote != null)
                {
                    PartyPredictions[i].ConstituencySeats = predictedResultPartyVote.ConstituencySeats;
                    PartyPredictions[i].ListSeats = predictedResultPartyVote.ListSeats;
                }
            }
        }

        private static ConstituencyResultProvider GetConstituencyResultProviderWithSwings(
            IConstituencyResultProvider provider,
            Dictionary<string, float> swingsByParty)
        {
            List<ConstituencyResult> resultsWithSwings = new List<ConstituencyResult>();
            foreach (string constituencyName in provider.ConstituencyNames)
            {
                resultsWithSwings.Add(
                    new ConstituencyResult(provider.ResultsByName[constituencyName], swingsByParty));
            }

            ConstituencyResultProvider predictedResultProvider =
                new ConstituencyResultProvider(resultsWithSwings);
            return predictedResultProvider;
        }

        private void SetupPartyVoteSwings()
        {
            Dictionary<string, PartyVote> partyVotesByAbbreviation = new Dictionary<string, PartyVote>();

            // Set up all the parties votes.
            foreach (PartyResult constituencyVote in OpinionPoll.ConstituencyPredictions)
            {
                if (partyVotesByAbbreviation.ContainsKey(constituencyVote.PartyAbbreviation))
                {
                    partyVotesByAbbreviation[constituencyVote.PartyAbbreviation].PercentageConstituencyVote =
                        constituencyVote.PercentageOfVotes;
                }
                else
                {
                    partyVotesByAbbreviation.Add(
                        constituencyVote.PartyAbbreviation,
                        new PartyVote
                        {
                            Abbreviation = constituencyVote.PartyAbbreviation,
                            PercentageConstituencyVote = constituencyVote.PercentageOfVotes,
                            FullName =
                                PreviousResult.Parties.PartiesByAbbreviation[constituencyVote.PartyAbbreviation]
                        });
                }
            }

            foreach (PartyResult listVote in OpinionPoll.ListPredictions)
            {
                string abbreviation = listVote.PartyAbbreviation;
                if (partyVotesByAbbreviation.ContainsKey(abbreviation))
                {
                    partyVotesByAbbreviation[abbreviation].PercentageListVote =
                        listVote.PercentageOfVotes;
                }
                else
                {
                    partyVotesByAbbreviation.Add(
                        listVote.PartyAbbreviation,
                        new PartyVote
                        {
                            Abbreviation = abbreviation,
                            PercentageListVote = listVote.PercentageOfVotes,
                            FullName =
                                PreviousResult.Parties.PartiesByAbbreviation.ContainsKey(abbreviation)
                                    ? PreviousResult.Parties.PartiesByAbbreviation[abbreviation]
                                    : abbreviation
                        });
                }
            }

            // Get the sorted votes.
            List<PartyVote> partyVotes = partyVotesByAbbreviation
                .Values
                .OrderByDescending(x => x.TotalSeats)
                .ThenByDescending(x => x.PercentageConstituencyVote)
                .ThenByDescending(x => x.PercentageListVote)
                .ToList();

            // Copy the values into the predictions
            PartyPredictions.Clear();
            foreach (PartyVote partyVote in partyVotes)
            {
                PartyPredictions.Add(new PartyVoteSwing(partyVote, PreviousResult));
            }
        }

        #endregion region

        public ElectionPrediction(
            ElectionResult previousResult,
            OpinionPoll opinionPoll)
        {
            PreviousResult = previousResult;
            OpinionPoll = opinionPoll;

            PartyPredictions = new List<PartyVoteSwing>();
            SetupPartyPredictions();
        }

        public ElectionPrediction(ElectionPrediction src)
        {
            PreviousResult = src.PreviousResult;
            OpinionPoll = new OpinionPoll(src.OpinionPoll);
            PredictedResult = src.PredictedResult;

            PartyPredictions = new List<PartyVoteSwing>();
            foreach (PartyVoteSwing partyPrediction in src.PartyPredictions)
            {
                PartyPredictions.Add(new PartyVoteSwing(partyPrediction));
            }
        }
    }
}
