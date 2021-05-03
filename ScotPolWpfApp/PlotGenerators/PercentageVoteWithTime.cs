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
    /// The percentage votes with time plot generator.
    /// </summary>
    public abstract class PercentageVoteWithTime : PollsWithTimePlot
    {
        #region Private Methods

        public override void UpdateModel(ref PlotModel newPlot)
        {
            OxyPlotUtilities.SetupPlotLegend(newPlot, PlotTitle);
            SetupListVotesVsTimeAxes(newPlot);

            // if no data stop
            if (ElectionPredictions?.PollingPredictions == null ||
                !ElectionPredictions.PollingPredictions.Any())
            {
                return;
            }

            // Get the polls by date.
            List<KeyValuePair<DateTime, PartyPrediction>> pollValues = 
                GetPartyPredictionsByDate();

            // Get the party names.
            string[] partiesNames = PartyPrediction.PartiesList;

            // Add a line and a scatter series per party
            List<KeyValuePair<string, LineSeries>> partiesLineSeries =
                new List<KeyValuePair<string, LineSeries>>();
            List<KeyValuePair<string, ScatterSeries>> partiesScatterSeries =
                new List<KeyValuePair<string, ScatterSeries>>();

            SetupPartiesSeries(partiesNames, partiesLineSeries, partiesScatterSeries);

            // loop through the polls adding points for each of the items to the lines
            AddPercentageValuesToSeries(pollValues, partiesLineSeries, partiesScatterSeries);

            // add them to the plot
            foreach (KeyValuePair<string, LineSeries> partyLines in partiesLineSeries)
            {
                newPlot.Series.Add(partyLines.Value);
            }

            foreach (KeyValuePair<string, ScatterSeries> partyScatters in partiesScatterSeries)
            {
                newPlot.Series.Add(partyScatters.Value);
            }
        }

        private void SetupPartiesSeries(
            string[] partiesNames, 
            List<KeyValuePair<string, LineSeries>> partiesLineSeries, 
            List<KeyValuePair<string, ScatterSeries>> partiesScatterSeries)
        {
            for (int i = 0; i < partiesNames.Length; i++)
            {
                string partyName = partiesNames[i];
                int colourIndex =
                    PartyNameToColoursLookup.ContainsKey(partyName)
                        ? PartyNameToColoursLookup[partyName]
                        : i;

                OxyPlotUtilities.CreateLongLineSeries(
                    out LineSeries partyLineSeries,
                    ChartAxisKeys.DateKey,
                    ChartAxisKeys.PercentageListVotes,
                    partiesNames[i],
                    colourIndex,
                    128,
                    strokeThickness: 3);

                partiesLineSeries.Add(
                    new KeyValuePair<string, LineSeries>(partiesNames[i], partyLineSeries));

                OxyPlotUtilities.CreateScatterSeries(
                    out ScatterSeries scatterSeries,
                    ChartAxisKeys.DateKey,
                    ChartAxisKeys.PercentageListVotes,
                    partiesNames[i],
                    colourIndex,
                    128,
                    false);

                partiesScatterSeries.Add(
                    new KeyValuePair<string, ScatterSeries>(partiesNames[i], scatterSeries));
            }
        }

        private static void AddPercentageValuesToSeries(
            List<KeyValuePair<DateTime, PartyPrediction>> pollValues, 
            List<KeyValuePair<string, LineSeries>> partiesLineSeries, 
            List<KeyValuePair<string, ScatterSeries>> partiesScatterSeries)
        {
            foreach (KeyValuePair<DateTime, PartyPrediction> pollValuePair in pollValues)
            {
                double date = DateTimeAxis.ToDouble(pollValuePair.Key);
                PartyPrediction partyPrediction = pollValuePair.Value;

                for (int i = 0; i < partiesLineSeries.Count; i++)
                {
                    string key = partiesLineSeries[i].Key;

                    double percentage =
                        partyPrediction.GetPartyValue(key);

                    partiesLineSeries[i].Value.Points.Add(new DataPoint(date, percentage));
                    partiesScatterSeries[i].Value.Points.Add(
                        new ScatterPoint(date, percentage));
                }
            }
        }

        private void SetupListVotesVsTimeAxes(PlotModel newPlot)
        {
            DateTimeAxis xAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Date",
                Key = ChartAxisKeys.DateKey,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.None,
                StringFormat = "yyyy-MM-dd"
            };

            newPlot.Axes.Add(xAxis);

            LinearAxis lhsAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = AxisTitle,
                Key = ChartAxisKeys.PercentageListVotes,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.None
            };

            newPlot.Axes.Add(lhsAxis);
        }

        #endregion

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

            newPlot.Background = OxyColors.Silver;
            newPlot.PlotAreaBackground = OxyColors.Gray;

            UpdateModel(ref newPlot);

            // finally update the model with the new plot
            return newPlot;
        }
        
        #endregion
    }
}
