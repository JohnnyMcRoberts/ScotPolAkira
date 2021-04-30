namespace ScotPolWpfApp.PlotGenerators
{
    using ElectionDataTypes.Polling;

    /// <summary>
    /// The list votes with time plot generator.
    /// </summary>
    public class ListVotesWithTime : PercentageVoteWithTime
    {
        public override string AxisTitle => "% List Support";
        public override string PlotTitle => "Predicted List Vote";
        public override PartyPrediction GetPartyPredictionValues(PollingPrediction prediction)
        {
            return prediction.ListPercentages; ;
        }
    }
}
