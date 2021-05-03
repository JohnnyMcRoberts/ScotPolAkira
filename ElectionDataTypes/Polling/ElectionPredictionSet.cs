using System.Linq;

namespace ElectionDataTypes.Polling
{
    using System;
    using System.Collections.Generic;
    
    using Interfaces;
    using Results;
    
    public class ElectionPredictionSet
    {
        #region Public Properties

        public List<ElectionPrediction> Predictions { get; set; }
        public List<PollingPrediction> PollingPredictions { get; set; }

        public List<PartyPollingResultsSet> PartyPollingSets { get; set; }
        public List<PollingPrediction> InterpolatedPredictions { get; set; }

        public event EventHandler PollsUpdated;

        #endregion

        #region Private Methods

        private void InitialisePartyPollingSets(IPollsProvider polls)
        {
            PartyPollingSets = new List<PartyPollingResultsSet>();
            foreach (string party in PartyPrediction.PartiesList)
            {
                PartyPollingSets.Add(
                    new PartyPollingResultsSet(party, true, polls.PollsByDate));
                PartyPollingSets.Add(
                    new PartyPollingResultsSet(party, false, polls.PollsByDate));
            }
        }

        private void SetupInterpolatedPredictions(
            IPollsProvider polls,
            ElectionResult previousElectionResult)
        {
            InterpolatedPredictions.Clear();
            if (!polls.PollsByDate.Any())
            {
                return;
            }

            // Make up a set of dummy polls from the interpolated values
            DateTime startDate = polls.PollsByDate.First().PublicationDate;
            DateTime lastDate = polls.PollsByDate.Last().PublicationDate;
            int numberOfInterpolatedValues = (lastDate - startDate).Days + 1;
            
            List<PartyPollingResultsSet> constituencyPredictors =
                PartyPollingSets.Where(x => x.IsConstituency).ToList();
            List<PartyPollingResultsSet> listPredictors =
                PartyPollingSets.Where(x => !x.IsConstituency).ToList();

            for (int i = 0; i < numberOfInterpolatedValues; i++)
            {
                OpinionPoll interpolationPoll = 
                    GetInterpolationPoll(polls, startDate, i, constituencyPredictors, listPredictors);
                
                ElectionPrediction electionPrediction =
                    new ElectionPrediction(previousElectionResult, interpolationPoll);
                InterpolatedPredictions.Add(new PollingPrediction(electionPrediction, interpolationPoll));
            }
        }

        private static OpinionPoll GetInterpolationPoll(
            IPollsProvider polls, 
            DateTime startDate, 
            int i,
            List<PartyPollingResultsSet> constituencyPredictors,
            List<PartyPollingResultsSet> listPredictors)
        {
            DateTime interpolatedDate = startDate.AddDays(i);

            // Get the interpolated percentages
            List<PartyResult> constituencyPredictions =
                constituencyPredictors.Select(
                    constituencyPredictor => new PartyResult
                    {
                        PartyAbbreviation = constituencyPredictor.PartyAbbreviation,
                        PercentageOfVotes = constituencyPredictor.GetInterploatedValue(interpolatedDate)
                    }).ToList();

            List<PartyResult> listPredictions =
                listPredictors.Select(
                    listPredictor => new PartyResult
                    {
                        PartyAbbreviation = listPredictor.PartyAbbreviation,
                        PercentageOfVotes = listPredictor.GetInterploatedValue(interpolatedDate)
                    }).ToList();

            // Make up the poll
            OpinionPoll interpolationPoll = new OpinionPoll
            {
                Link = polls.PollsByDate.First().Link,
                PollingCompany = "@Interpolation",
                PublicationDate = interpolatedDate,
                ConstituencyPredictions = constituencyPredictions,
                ListPredictions = listPredictions
            };

            return interpolationPoll;
        }

        #endregion

        #region Public Methods

        public virtual void OnPollsUpdated(EventArgs e)
        {
            EventHandler handler = PollsUpdated;
            handler?.Invoke(this, e);
        }

        public void UpdatePredictions(
            IPollsProvider polls,
            ElectionResult previousElectionResult)
        {
            Predictions.Clear();

            foreach (OpinionPoll poll in polls.PollsByDate)
            {
                ElectionPrediction electionPrediction =
                    new ElectionPrediction(previousElectionResult, poll);
                Predictions.Add(electionPrediction);
                PollingPredictions.Add(new PollingPrediction(electionPrediction , poll));
            }

            InitialisePartyPollingSets(polls);

            SetupInterpolatedPredictions(polls, previousElectionResult);

            OnPollsUpdated(null);
        }

        #endregion

        public ElectionPredictionSet()
        {
            Predictions = new List<ElectionPrediction>();
            PollingPredictions = new List<PollingPrediction>();

            PartyPollingSets = new List<PartyPollingResultsSet>();
            InterpolatedPredictions = new List<PollingPrediction>();
        }
    }
}
