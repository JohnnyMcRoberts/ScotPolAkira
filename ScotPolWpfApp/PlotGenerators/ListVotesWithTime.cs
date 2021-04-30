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
    /// The list votes with time plot generator.
    /// </summary>
    public class ListVotesWithTime : BasePlotGenerator
    {
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
                    Title = "List votes with time Plot at "+ DateTime.Now.ToLongTimeString()
                };
            
            UpdateModel(ref newPlot);

            // finally update the model with the new plot
            return newPlot;
        }

        public bool UpdateModel(ref PlotModel newPlot)
        {
            OxyPlotUtilities.SetupPlotLegend(
                newPlot, "List votes with time Plot at " + DateTime.Now.ToLongTimeString());
            SetupListVotesVsTimeAxes(newPlot);

            // if no data stop
            if (ElectionPredictions?.PollingPredictions == null ||
                !ElectionPredictions.PollingPredictions.Any())
            {
                return true;
            }

            // Get the polls by date.
            List<KeyValuePair<DateTime, PartyPrediction>> pollValues =
                new List<KeyValuePair<DateTime, PartyPrediction>>();

            foreach (PollingPrediction prediction in ElectionPredictions.PollingPredictions)
            {
                pollValues.Add(
                    new KeyValuePair<DateTime, PartyPrediction>(
                        prediction.PublicationDate, prediction.ListPercentages));
            }

            // Get the party names.
            string[] partiesNames =
                ElectionPredictions.PollingPredictions.First().ListSeats.PartiesList;

            // Add a series per party
            List<KeyValuePair<string, LineSeries>> partiesLineSeries =
                new List<KeyValuePair<string, LineSeries>>();

            for (int i = 0; i < partiesNames.Length; i++)
            {
                OxyPlotUtilities.CreateLongLineSeries(
                    out LineSeries partyLineSeries,
                    ChartAxisKeys.DateKey,
                    ChartAxisKeys.PercentageListVotes,
                    partiesNames[i],
                    i,
                    128);

                partiesLineSeries.Add(
                    new KeyValuePair<string, LineSeries>(partiesNames[i], partyLineSeries));
            }

            // loop through the pols adding points for each of the items to the lines
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
                }
            }

            // add them to the plot
            foreach (KeyValuePair<string, LineSeries> countryItems in partiesLineSeries)
            {
                newPlot.Series.Add(countryItems.Value);
            }

            return false;
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
                Title = "Total Books Read",
                Key = ChartAxisKeys.PercentageListVotes,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.None
            };

            newPlot.Axes.Add(lhsAxis);
        }
    }
}
