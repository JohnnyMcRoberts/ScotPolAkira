using System;
using System.Collections.Generic;
using System.Text;

namespace ScotPolWpfApp.PlotGenerators
{
    using ElectionDataTypes.Polling;

    /// <summary>
    /// The percentage votes with time plot generator.
    /// </summary>
    public class TotalSeatsWithTime : PercentageVoteWithTime
    {
        public override string AxisTitle => "Total Seats";
        public override string PlotTitle => "Predicted Seats by Time";
        public override PartyPrediction GetPartyPredictionValues(PollingPrediction prediction)
        {
            return prediction.TotalSeats ;
        }

    }
}
