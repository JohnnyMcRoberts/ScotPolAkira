namespace ScotPolWpfApp.PlotGenerators
{
    using ElectionDataTypes.Polling;
    using ElectionDataTypes.Results;

    using OxyPlot;

    using Interfaces;

    /// <summary>
    /// The base plot generator class.
    /// </summary>
    public abstract class BasePlotGenerator : IPlotGenerator
    {
        /// <summary>
        /// Gets the geography data for the plots.
        /// </summary>
        public ElectionResult ElectionResults { get; private set; }

        /// <summary>
        /// Gets the books read data for the plots.
        /// </summary>
        public ElectionPredictionSet ElectionPredictions { get; private set; }

        public PlotModel SetupPlot(
            ElectionResult electionResults, ElectionPredictionSet electionPredictions)
        {
            ElectionResults = electionResults;
            ElectionPredictions = electionPredictions;
            return SetupPlot();
        }

        /// <summary>
        /// Sets up the plot model to be displayed.
        /// </summary>
        /// <returns>The plot model.</returns>
        protected abstract PlotModel SetupPlot();
    }
}
