namespace ScotPolWpfApp.PlotGenerators
{
    using OxyPlot;
    using OxyPlot.Annotations;

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

        #region Private Methods

        public override void UpdateModel(ref PlotModel newPlot)
        {
            // Update the basic plot
            base.UpdateModel(ref newPlot);

            // add a line annotation
            double X = 5D;
            double Y = 65D;

            LineAnnotation lineAnnotation = new LineAnnotation()
            {
                StrokeThickness = 3,
                Color = OxyColors.White,
                Type = LineAnnotationType.Horizontal,
                TextRotation = 45,
                TextOrientation = AnnotationTextOrientation.AlongLine,
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Text = "Majority",
                TextColor = OxyColors.White,
                X = X,
                Y = Y
            };

            newPlot.Annotations.Add(lineAnnotation);
        }

        #endregion
    }
}
