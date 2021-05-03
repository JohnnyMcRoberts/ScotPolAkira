
namespace ScotPolWpfApp.Utilities
{
    using System;
    using System.Collections.Generic;
    using OxyPlot;
    using OxyPlot.Series;

    public class OxyPlotUtilities
    {
        #region Constants

        // taken from http://dmcritchie.mvps.org/excel/colors.htm
        private static readonly List<Tuple<byte, byte, byte>> StandardColours =
            new List<Tuple<byte, byte, byte>>
            {
                                    // R    G    B
            new Tuple<byte,byte,byte>(255,  0   ,0),
            new Tuple<byte,byte,byte>(0,    255 ,0),
            new Tuple<byte,byte,byte>(0,    0   ,255),
            new Tuple<byte,byte,byte>(255,  255 ,0),
            new Tuple<byte,byte,byte>(255,  0   ,255),
            new Tuple<byte,byte,byte>(0,    255 ,255),
            new Tuple<byte,byte,byte>(128,  0   ,0),
            new Tuple<byte,byte,byte>(0,    128 ,0),
            new Tuple<byte,byte,byte>(0,    0,  128),
            new Tuple<byte,byte,byte>(128,  128 ,0),
            new Tuple<byte,byte,byte>(128,  0   ,128),
            new Tuple<byte,byte,byte>(0,    128 ,128),
            new Tuple<byte,byte,byte>(153,  153 ,255),
            new Tuple<byte,byte,byte>(153,  51  ,102),
            new Tuple<byte,byte,byte>(255,  255 ,204),
            new Tuple<byte,byte,byte>(204,  255 ,255),
            new Tuple<byte,byte,byte>(102,  0   ,102),
            new Tuple<byte,byte,byte>(255,  128 ,128),
            new Tuple<byte,byte,byte>(0,    102 ,204),
            new Tuple<byte,byte,byte>(204,  204 ,255),
            new Tuple<byte,byte,byte>(0,    0   ,128),
            new Tuple<byte,byte,byte>(255,  0   ,255),
            new Tuple<byte,byte,byte>(255,  255 ,0),
            new Tuple<byte,byte,byte>(0,    255 ,255),
            new Tuple<byte,byte,byte>(128,  0   ,128),
            new Tuple<byte,byte,byte>(128,  0   ,0),
            new Tuple<byte,byte,byte>(0,    128 ,128),
            new Tuple<byte,byte,byte>(0,    0   ,255),
            new Tuple<byte,byte,byte>(0,    204 ,255),
            new Tuple<byte,byte,byte>(204,  255 ,255),
            new Tuple<byte,byte,byte>(204,  255 ,204),
            new Tuple<byte,byte,byte>(255,  255 ,153),
            new Tuple<byte,byte,byte>(153,  204 ,255),
            new Tuple<byte,byte,byte>(255,  153 ,204),
            new Tuple<byte,byte,byte>(204,  153 ,255),
            new Tuple<byte,byte,byte>(255,  204 ,153),
            new Tuple<byte,byte,byte>(51,   102 ,255),
            new Tuple<byte,byte,byte>(51,   204 ,204),
            new Tuple<byte,byte,byte>(153,  204 ,0),
            new Tuple<byte,byte,byte>(255,  204 ,0),
            new Tuple<byte,byte,byte>(255,  153 ,0),
            new Tuple<byte,byte,byte>(255,  102 ,0),
            new Tuple<byte,byte,byte>(102,  102 ,153),
            new Tuple<byte,byte,byte>(150,  150 ,150),
            new Tuple<byte,byte,byte>(0,    51  ,102),
            new Tuple<byte,byte,byte>(51,   153 ,102),
            new Tuple<byte,byte,byte>(0,    51  ,0),
            new Tuple<byte,byte,byte>(51,  51   ,0),
            new Tuple<byte,byte,byte>(153,  51  ,0),
            new Tuple<byte,byte,byte>(153,  51  ,102),
            new Tuple<byte,byte,byte>(51,   51  ,153),
        };

        #endregion

        public static void SetupPlotLegend(PlotModel newPlot,
            string title = "Performance Curves")
        {
            newPlot.LegendTitle = title;
            newPlot.LegendOrientation = LegendOrientation.Horizontal;
            newPlot.LegendPlacement = LegendPlacement.Outside;
            newPlot.LegendPosition = LegendPosition.TopRight;
            newPlot.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            newPlot.LegendBorder = OxyColors.Black;
            newPlot.LegendMargin = 20;
        }

        public static List<OxyColor> SetupStandardColourSet(byte aValue = 225)
        {
            List<OxyColor> standardColours = new List<OxyColor>();

            foreach (Tuple<byte, byte, byte> colour in StandardColours)
            {
                standardColours.Add(OxyColor.FromArgb(aValue, colour.Item1, colour.Item2, colour.Item3));
            }

            return standardColours;
        }

        public static void CreateLongLineSeries(
            out LineSeries series, 
            string xAxisKey,
            string yAxisKey, 
            string title, 
            int colourIndex,
            byte aValue = 225,
            double? strokeThickness = null)
        {
            List<OxyColor> coloursArray = SetupStandardColourSet(aValue);

            int index = colourIndex % coloursArray.Count;
            OxyColor colour = coloursArray[index];

            series = new LineSeries
            {
                Title = title,
                XAxisKey = xAxisKey,
                YAxisKey = yAxisKey,
                Color = colour
            };

            if (strokeThickness.HasValue)
            {
                series.StrokeThickness = strokeThickness.Value;
            }
        }

        public static void CreateScatterSeries(
            out ScatterSeries series,
            string xAxisKey,
            string yAxisKey,
            string title,
            int colourIndex,
            byte aValue = 225,
            bool includeInLegend = true)
        {
            List<OxyColor> coloursArray = SetupStandardColourSet(aValue);

            int index = colourIndex % coloursArray.Count;
            OxyColor colour = coloursArray[index];

            series = new ScatterSeries
            {
                Title = includeInLegend ? title : string.Empty,
                XAxisKey = xAxisKey,
                YAxisKey = yAxisKey,
                MarkerSize = 5,
                MarkerType = MarkerType.Circle,
                MarkerFill = colour
            };
        }

        public static IEnumerable<AreaSeries> StackLineSeries(
            IList<LineSeries> series)
        {
            double[] total = new double[series[0].Points.Count];

            for (int s = 0; s < series.Count; s++)
            {
                LineSeries lineSeries = series[s];
                AreaSeries areaSeries = new AreaSeries()
                {
                    Title = lineSeries.Title,
                    Color = lineSeries.Color,
                };

                for (int p = 0; p < lineSeries.Points.Count; p++)
                {
                    double x = lineSeries.Points[p].X;
                    double y = lineSeries.Points[p].Y;

                    areaSeries.Points.Add(new DataPoint(x, total[p]));
                    total[p] += y;
                    areaSeries.Points2.Add(new DataPoint(x, total[p]));
                }

                yield return areaSeries;
            }
        }
        }
}
