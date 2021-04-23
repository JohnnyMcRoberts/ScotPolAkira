﻿namespace ElectionDataTypes.Polling
{
    using System.Collections.Generic;
    
    using Interfaces;
    using Results;
    
    public class ElectionPredictionSet
    {
        public List<ElectionPrediction> Predictions { get; set; }

        public void UpdatePredictions(IPollsProvider polls, ElectionResult previousElectionResult)
        {
            Predictions.Clear();

            foreach (var poll in polls.PollsByDate)
            {
                Predictions.Add(new ElectionPrediction(previousElectionResult, poll));
            }
        }

        public ElectionPredictionSet()
        {
            Predictions = new List<ElectionPrediction>();
        }
    }
}