using Cross.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
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
                return PurposeColor.WinPhone.App.Current.Host.Content.ActualWidth;
            }
        }

        public double ScreenHeight 
        {
            get
            {
                return PurposeColor.WinPhone.App.Current.Host.Content.ActualHeight;
            }
        }


        public double ScreenDensity
        {
            get
            {
                double density = PurposeColor.WinPhone.App.Current.Host.Content.ScaleFactor / 100;
                return density;
            }
        }
    }
}
