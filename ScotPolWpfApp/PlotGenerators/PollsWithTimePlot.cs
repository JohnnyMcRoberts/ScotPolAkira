namespace ScotPolWpfApp.PlotGenerators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    using ElectionDataTypes.Polling;

    using Utilities;

    /// <summary>
    /// The polls with time plot generator.
    /// </summary>
    public abstract class PollsWithTimePlot : BasePlotGenerator
    {
        public abstract string AxisTitle { get; }

        public abstract string PlotTitle { get; }

        public abstract PartyPrediction GetPartyPredictionValues(PollingPrediction prediction);

        public abstract void UpdateModel(ref PlotModel newPlot);

        public List<KeyValuePair<DateTime, PartyPrediction>> GetPartyPredictionsByDate()
        {
            List<KeyValuePair<DateTime, PartyPrediction>> pollValues =
                new List<KeyValuePair<DateTime, PartyPrediction>>();

            foreach (PollingPrediction prediction in ElectionPredictions.PollingPredictions)
            {
                PartyPrediction partyPredictionValues = GetPartyPredictionValues(prediction);

                pollValues.Add(
                    new KeyValuePair<DateTime, PartyPrediction>(
                        prediction.PublicationDate, partyPredictionValues));
            }

            return pollValues;
        }

        #region Protected Overriden Methods

        /// <summary>
        /// Sets up the plot model to be displayed.
        /// </summary>
        /// <returns>The plot model.</returns>
        protected override PlotModel SetupPlot()
        {
            // Create the plot model
            PlotModel newPlot =
                new PlotModel
                {
                    Title = PlotTitle
                };

            UpdateModel(ref newPlot);

            // finally update the model with the new plot
            return newPlot;
        }

        #endregion
    }
}
