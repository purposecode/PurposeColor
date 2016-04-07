using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PurposeColor.interfaces
{
    public interface IResize
    {
		byte[] Resize(byte[] imageData, float width, float height, string path);

        MemoryStream CompessImage(int ratio, MemoryStream ms);

	    Size GetImageSize (string path);
    }
}
