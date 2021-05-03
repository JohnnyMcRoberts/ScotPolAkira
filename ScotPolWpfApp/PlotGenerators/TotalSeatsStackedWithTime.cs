using OxyPlot.Annotations;

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
    /// The stacked seats with time plot generator.
    /// </summary>
    public class TotalSeatsStackedWithTime : PollsWithTimePlot
    {
        public override string AxisTitle => "Total Seats";

        public override string PlotTitle => "Predicted Total Seats";

        public override PartyPrediction GetPartyPredictionValues(PollingPrediction prediction)
        {
            return prediction.TotalSeats;
        }

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
        
        #region Private Methods

        public override void UpdateModel(ref PlotModel newPlot)
        {
            OxyPlotUtilities.SetupPlotLegend(newPlot, PlotTitle);
            SetupTotalSeatsVsTimeAxes(newPlot);

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

            SetupPartiesSeries(partiesNames, partiesLineSeries);

            // loop through the polls adding points for each of the items to the lines
            AddValuesToSeries(pollValues, partiesLineSeries);

            // Get the area stack series
            IEnumerable<AreaSeries> stackedSeatsSeries = 
                OxyPlotUtilities.StackLineSeries(partiesLineSeries.Select(x => x.Value).ToList());

            // add them to the plot
            foreach (AreaSeries series in stackedSeatsSeries)
            {
                newPlot.Series.Add(series);
            }

            // add a line annotation
            double X = 5D;
            double Y =65D;

            LineAnnotation lineAnnotation = new LineAnnotation()
            {
                StrokeThickness = 3,
                Color = OxyColors.Crimson,
                Type = LineAnnotationType.Horizontal,
                Text = "Majority",
                TextColor = OxyColors.Crimson,
                X = X,
                Y = Y
            };

            newPlot.Annotations.Add(lineAnnotation);
        }

        private void AddValuesToSeries(
            List<KeyValuePair<DateTime, PartyPrediction>> pollValues,
            List<KeyValuePair<string, LineSeries>> partiesLineSeries)
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
                }
            }
        }

        private void SetupPartiesSeries(string[] partiesNames, List<KeyValuePair<string, LineSeries>> partiesLineSeries)
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
                    128);

                partiesLineSeries.Add(
                    new KeyValuePair<string, LineSeries>(partiesNames[i], partyLineSeries));
            }
        }

        private void SetupTotalSeatsVsTimeAxes(PlotModel newPlot)
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
                Key = ChartAxisKeys.TotalPredictedSeats,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.None
            };

            newPlot.Axes.Add(lhsAxis);
        }

        #endregion
    }
}
