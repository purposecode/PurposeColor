using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{
    public class CustomeGraphModel
    {

        /// <summary>
        /// Gets or sets the plot model that is shown in the demo apps.
        /// </summary>
        /// <value>My model.</value>
        public PlotModel plotModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlotSample.MyClass"/> class.
        /// </summary>
        public CustomeGraphModel()
        {
            plotModel = new PlotModel
            {
                Title = "Emotional zone detailed analysis"
            };

            //plotModel.Axes.Add(new LinearAxis { PositionAtZeroCrossing = true, IsZoomEnabled = false, IsPanEnabled = false, Position = AxisPosition.Bottom, Minimum = -2, Maximum = 2, TickStyle = TickStyle.None, AxislineColor = OxyColors.Transparent });
            //plotModel.Axes.Add(new LinearAxis { PositionAtZeroCrossing = true, IsZoomEnabled = false, IsPanEnabled = false, Position = AxisPosition.Left, Minimum = -2, Maximum = 2, TickStyle = TickStyle.None, AxislineColor = OxyColors.Transparent });

            #region LINE SERIES
            /*
            var xaxis = new LinearAxis
            {
                Position = AxisPosition.Bottom
            };

            var yaxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 10
            };



            plotModel.Axes.Add(xaxis);
            plotModel.Axes.Add(yaxis);

            var series1 = new LineSeries
            {
                StrokeThickness = 3,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White,
                MarkerStrokeThickness = 1
            };

            series1.Points.Add(new DataPoint(0.0, 6.0));
            series1.Points.Add(new DataPoint(1.4, 2.1));
            series1.Points.Add(new DataPoint(2.0, 4.2));
            series1.Points.Add(new DataPoint(3.3, 2.3));
            series1.Points.Add(new DataPoint(4.7, 7.4));
            series1.Points.Add(new DataPoint(6.0, 6.2));
            series1.Points.Add(new DataPoint(8.0, 8.9));

            plotModel.Series.Add(series1);
            */
            #endregion

            #region PIE SERIES DUNMMY PLOT MODEL
            
            // http://www.nationsonline.org/oneworld/world_population.htm
            // http://en.wikipedia.org/wiki/Continent

            var pieSeries = new PieSeries();

            //in actual implementation PieSlice has to be derived from a dictionary or comining arrays.
            pieSeries.Slices.Add(new PieSlice("Joy", 1030) { IsExploded = true });
            pieSeries.Slices.Add(new PieSlice("Sadness", 929) { IsExploded = true });
            pieSeries.Slices.Add(new PieSlice("Trust", 4157));
            pieSeries.Slices.Add(new PieSlice("Anticipation", 939) { IsExploded = true });
            
            pieSeries.InnerDiameter = 0.5;
            pieSeries.ExplodedDistance = 0.0;
            pieSeries.Stroke = OxyColors.White;
            pieSeries.StrokeThickness = 2.0;
            pieSeries.InsideLabelPosition = 0.8;
            pieSeries.AngleSpan = 360;
            pieSeries.StartAngle = 0;
            pieSeries.TickRadialLength = 1;
            pieSeries.ToolTip = "";
            pieSeries.TextColor = OxyColor.FromRgb(1, 1, 1);
            pieSeries.InsideLabelColor = OxyColor.FromRgb(5, 5, 5);


            //pieSeries.OutsideLabelFormat = null;  /// the percentage display of each slice.

            plotModel.Background = OxyColors.Transparent;
            plotModel.PlotAreaBorderColor = OxyColors.Transparent;

            plotModel.Series.Add(pieSeries);
            
            #endregion

            
        }
    }
}
