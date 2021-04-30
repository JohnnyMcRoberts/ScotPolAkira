namespace ScotPolWpfApp.Interfaces
{
    using OxyPlot;
    using ElectionDataTypes.Polling;
    using ElectionDataTypes.Results;
    
    public interface IPlotGenerator
    {
        PlotModel SetupPlot(
            ElectionResult electionResults, ElectionPredictionSet electionPredictions);
    }
}
