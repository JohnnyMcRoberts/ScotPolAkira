namespace ElectionDataTypes.Polling
{
    using System;
    using System.Collections.Generic;
    
    using Interfaces;
    using Results;
    
    public class ElectionPredictionSet
    {
        public List<ElectionPrediction> Predictions { get; set; }
        public List<PollingPrediction> PollingPredictions { get; set; }

        public event EventHandler PollsUpdated;

        protected virtual void OnPollsUpdated(EventArgs e)
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

            OnPollsUpdated(null);
        }

        public ElectionPredictionSet()
        {
            Predictions = new List<ElectionPrediction>();
            PollingPredictions = new List<PollingPrediction>();
        }
    }
}
