using Microsoft.Phone;
using PurposeColor.interfaces;
using PurposeColor.WinPhone.Dependency;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Xamarin.Forms;

[assembly: Dependency(typeof(WinResize))]
namespace PurposeColor.WinPhone.Dependency
{
    public class WinResize : IResize
    {
        public byte[] Resize(byte[] imageData, float width, float height)
        {
            ResizeImageWinPhone(imageData, width, height);
            return null; //for testing only
        }

        public static byte[] ResizeImageWinPhone(byte[] imageData, float width, float height)
        {
            byte[] resizedData;

            using (MemoryStream streamIn = new MemoryStream(imageData))
            {
                WriteableBitmap bitmap = PictureDecoder.DecodeJpeg(streamIn, (int)width, (int)height);

                using (MemoryStream streamOut = new MemoryStream())
                {
                    bitmap.SaveJpeg(streamOut, (int)width, (int)height, 0, 100);
                    resizedData = streamOut.ToArray();
                }
            }
            return resizedData;
        }

        public MemoryStream CompessImage(int ratio, MemoryStream ms)
        {
            return null; //for testing only
        }

    }
}
