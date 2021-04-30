namespace ScotPolWpfApp.PlotGenerators
{
    using ElectionDataTypes.Polling;

    /// <summary>
    /// The constituency votes with time plot generator.
    /// </summary>
    public class ConstituencyVotesWithTime : PercentageVoteWithTime
    {
        public override string AxisTitle => "% Constituency Support";
        public override string PlotTitle => "Predicted Constituency Vote";
        public override PartyPrediction GetPartyPredictionValues(PollingPrediction prediction)
        {
            return prediction.ConstituencyPercentages;
        }
    }
}
