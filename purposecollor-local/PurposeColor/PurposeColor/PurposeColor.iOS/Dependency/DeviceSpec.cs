using System;
using Cross;
using UIKit;
using Xamarin.Forms;
using PurposeColor.iOS;

[assembly: Dependency(typeof(DeviceSpec))]
namespace PurposeColor.iOS
{
	class DeviceSpec : IDeviceSpec
	{
		public double ScreenWidth 
		{
			get
			{
				return UIScreen.MainScreen.Bounds.Width;
			}
		}

		public double ScreenHeight 
		{
			get
			{
				return UIScreen.MainScreen.Bounds.Height;
			}
		}


		public double ScreenDensity
		{
			get
			{
				return UIScreen.MainScreen.Scale;
			}
		}

		private int ConvertPixelsToDp(float pixelValue)
		{
			var dp = 100; //(int)((pixelValue) / Resources.System.DisplayMetrics.Density);
			return dp;
		}
	}
}

