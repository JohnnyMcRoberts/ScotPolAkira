namespace ScotPolWpfApp.PlotGenerators
{
    using System.Collections.Generic;

    using ElectionDataTypes.Polling;
    using ElectionDataTypes.Results;

    using OxyPlot;

    using Interfaces;

    /// <summary>
    /// The base plot generator class.
    /// </summary>
    public abstract class BasePlotGenerator : IPlotGenerator
    {
        #region Constants

        public readonly Dictionary<string, int> PartyNameToColoursLookup =
            new Dictionary<string, int>
            {
                {"LAB", 0},
                {"SGRN", 1},
                {"CON", 2},
                {"SNP", 3},
                {"LIB", 40},
                {"ALBA", 5}
            };

        #endregion

        /// <summary>
        /// Gets the previous election results for the plots.
        /// </summary>
        public ElectionResult ElectionResults { get; private set; }

        /// <summary>
        /// Gets the polling predictions for the plots.
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
