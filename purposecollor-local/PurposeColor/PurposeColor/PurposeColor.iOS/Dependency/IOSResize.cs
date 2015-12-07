using CoreGraphics;
using PurposeColor.interfaces;
using PurposeColor.iOS.Dependency;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UIKit;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(IOSResize))]
namespace PurposeColor.iOS.Dependency
{
    public class IOSResize : IResize
    {
		public byte[] Resize(byte[] imageData, float width, float height)
        {
            return ResizeImageIOS(imageData, width, height);
        }

        public static byte[] ResizeImageIOS(byte[] imageData, float width, float height)
        {
            UIImage originalImage = ImageFromByteArray(imageData);

            //create a 24bit RGB image
            using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
                (int)width, (int)height, 8,
                (int)(4 * width), CGColorSpace.CreateDeviceRGB(),
                CGImageAlphaInfo.PremultipliedFirst))
            {

                RectangleF imageRect = new RectangleF(0, 0, width, height);

                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);

                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage());

                // save the image as a jpeg
                return resizedImage.AsJPEG().ToArray();
            }
        }


		public MemoryStream CompessImage(int ratio, MemoryStream ms)
		{
			MemoryStream compressedStream = new MemoryStream();
			compressedStream = EZCompress1.Plugin.CrossEZCompress1.Current.compressImage(ms, 50);
			return compressedStream;
		}


        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
    }
}
