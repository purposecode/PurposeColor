using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PurposeColor.Helpers
{
    class ImageName
    {
        public static string Path( string imgName )
        {
            string winPath = "//Assets//";
            return Device.OnPlatform(imgName, imgName, winPath + imgName);
        }
    }
}
