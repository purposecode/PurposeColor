using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;

namespace PurposeColor.Model
{
	public class CustomeGraphModel
	{
		public PlotModel plotModel { get; set; }

		public CustomeGraphModel(List<EmotionValues> emotions)
		{
			plotModel = new PlotModel
			{
				Title = ""
			};
			int warmCount = emotions.Where (e => e.emotion_value == "-2").ToList ().Count;
			int assertiveCount = emotions.Where (e => e.emotion_value == "-1").ToList ().Count;
			int patientCount = emotions.Where (e => e.emotion_value == "1").ToList ().Count;
			int detailedCount = emotions.Where (e => e.emotion_value == "2").ToList ().Count;

			//plotModel.Axes.Add(new LinearAxis { PositionAtZeroCrossing = true, IsZoomEnabled = false, IsPanEnabled = false, Position = AxisPosition.Bottom, Minimum = -2, Maximum = 2, TickStyle = TickStyle.None, AxislineColor = OxyColors.Transparent });
			//plotModel.Axes.Add(new LinearAxis { PositionAtZeroCrossing = true, IsZoomEnabled = false, IsPanEnabled = false, Position = AxisPosition.Left, Minimum = -2, Maximum = 2, TickStyle = TickStyle.None, AxislineColor = OxyColors.Transparent });

			#region PIE SERIES DUNMMY PLOT MODEL

			var pieSeries = new PieSeries();

			//in actual implementation PieSlice has to be derived from a dictionary or comining arrays.
			pieSeries.Slices.Add(new PieSlice("Warm", warmCount) { IsExploded = true , Fill = OxyColor.FromRgb(249,68,0)});
			pieSeries.Slices.Add(new PieSlice("Patient", patientCount) { IsExploded = true , Fill = OxyColor.FromRgb(30,163,1)}); //31, 145, 2
			pieSeries.Slices.Add(new PieSlice("Assertive", assertiveCount){ IsExploded = true, Fill = OxyColor.FromRgb(255,201,60)});
			pieSeries.Slices.Add(new PieSlice("Detailed", detailedCount) { IsExploded = true, Fill = OxyColor.FromRgb(2,76,234)});

			pieSeries.InnerDiameter = 0; //0.3;
			pieSeries.ExplodedDistance = 0.0;
			pieSeries.Stroke = OxyColors.Transparent;
			pieSeries.StrokeThickness = 0;
			pieSeries.InsideLabelPosition = 0.6; // 0.8; -for stright label
			pieSeries.FontSize = App.screenDensity >= 2 ? Device.OnPlatform(10,22,22) : Device.OnPlatform(6,12,16);
			pieSeries.AngleSpan = 360;
			pieSeries.StartAngle = 0;
			pieSeries.TickRadialLength = 2;
			pieSeries.ToolTip = "My Emotional Zone";
			pieSeries.TextColor = OxyColor.FromRgb(1,1,1);
			pieSeries.InsideLabelColor = OxyColor.FromRgb(1,1,1);
			pieSeries.OutsideLabelFormat = null;  /// the percentage display of each slice.
			pieSeries.AreInsideLabelsAngled = true;

			plotModel.Background = OxyColors.Transparent;
			plotModel.PlotAreaBorderColor = OxyColors.Transparent;

			plotModel.Series.Add(pieSeries);

			#endregion


		}
	}
}
