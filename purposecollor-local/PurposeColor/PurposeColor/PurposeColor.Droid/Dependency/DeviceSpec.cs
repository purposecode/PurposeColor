using Android.Content.Res;
using Cross.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
[assembly: Dependency(typeof(DeviceSpec))]
namespace Cross.Store
{
    class DeviceSpec : IDeviceSpec
    {
        public double ScreenWidth 
        {
            get
            {
                return ConvertPixelsToDp(Resources.System.DisplayMetrics.WidthPixels);
            }
        }

        public double ScreenHeight 
        {
            get
            {
                return ConvertPixelsToDp(Resources.System.DisplayMetrics.HeightPixels);
            }
        }


        public double ScreenDensity
        {
            get
            {
                return Resources.System.DisplayMetrics.ScaledDensity;
            }
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.System.DisplayMetrics.Density);
            return dp;
        }
    }
}
