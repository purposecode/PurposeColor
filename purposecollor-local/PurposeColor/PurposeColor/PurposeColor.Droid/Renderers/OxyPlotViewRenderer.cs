using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.OS;
using OxyPlot;
using OxyPlot.Xamarin.Android;
using PurposeColor.screens;
using PurposeColor.Droid.Renderers;

[assembly: ExportRenderer (typeof (OxyPlotView), typeof (OxyPlotViewRenderer))]

namespace PurposeColor.Droid.Renderers
{
	public class OxyPlotViewRenderer : ViewRenderer
	{
		protected PlotView NativeControl
		{
			get { return ((PlotView) Control); }
		}
		protected OxyPlotView NativeElement
		{
			get { return ((OxyPlotView) Element); }
		}

		protected override void OnElementChanged (ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);

			var plotView = new PlotView (Context);

			NativeElement.OnInvalidateDisplay = (s,ea) => {
				plotView.Invalidate();
			};

			SetNativeControl (plotView);

			NativeControl.Model = NativeElement.Model;

			NativeControl.SetBackgroundColor (NativeElement.BackgroundColor.ToAndroid ());
		}

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			base.OnLayout (changed, l, t, r, b);
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
		
			if (e.PropertyName == OxyPlotView.BackgroundColorProperty.PropertyName) {
				NativeControl.SetBackgroundColor (NativeElement.BackgroundColor.ToAndroid ());
			}
		}
	}
}

