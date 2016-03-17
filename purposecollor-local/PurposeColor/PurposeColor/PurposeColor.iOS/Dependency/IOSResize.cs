using CoreGraphics;
using PurposeColor.interfaces;
using PurposeColor.iOS.Dependency;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UIKit;
using System.IO;
using Foundation;
using AssetsLibrary;
using Foundation;
using CoreFoundation;
using ImageIO;

[assembly: Xamarin.Forms.Dependency(typeof(IOSResize))]
namespace PurposeColor.iOS.Dependency
{
    public class IOSResize : IResize
    {
		public byte[] Resize(byte[] imageData, float width, float height, string path)
        {
			return ResizeImageIOS(imageData, width, height, path);
        }

		public static byte[] ResizeImageIOS(byte[] imageData, float width, float height, string path)
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


				var url = new NSUrl( path , false); 
				CGImageSource myImageSource;
				myImageSource = CGImageSource.FromUrl (url, null);
				var ns = new NSDictionary();
				NSDictionary imageProperties = myImageSource.CopyProperties(ns, 0);

				Console.WriteLine(imageProperties.DescriptionInStringsFileFormat);
				NSObject[] keys =  imageProperties.Keys;
				NSObject[] values =  imageProperties.Values;

				string orientation =  values [7].ToString ();
				if (orientation.Contains("6")) {
					resizedImage = UIImage.FromImage (resizedImage.CGImage, resizedImage.CurrentScale, UIImageOrientation.Right);
					Console.WriteLine("----img orientation------ 6 -------");
				}


				
				
                // save the image as a jpeg
				return resizedImage.AsJPEG().ToArray();
            }
        }


		public UIImage RotateImage( UIImage imageToRotate, bool isCCW)
		{
			UIImage rotatedImage = new UIImage ();

			var imageRotation = isCCW ? UIImageOrientation.Right : UIImageOrientation.Left;
			rotatedImage = UIImage.FromImage(imageToRotate.CGImage, imageToRotate.CurrentScale, UIImageOrientation.Right);
			return rotatedImage;
		}  


		public static UIImage ScaleAndRotateImage( UIImage image)
		{
			int kMaxResolution = 1280; // Or whatever

			CGImage imgRef = image.CGImage;
			float width = imgRef.Width;
			float height = imgRef.Height;
			CGAffineTransform transform = CGAffineTransform.MakeIdentity();
			RectangleF bounds = new RectangleF(0, 0, width, height);

			if (width > kMaxResolution || height > kMaxResolution)
			{
				float ratio = width / height;

				if (ratio > 1)
				{
					bounds.Size = new SizeF(kMaxResolution, bounds.Size.Width / ratio);
				}
				else
				{
					bounds.Size = new SizeF(bounds.Size.Height * ratio, kMaxResolution);
				}
			}

			float scaleRatio = bounds.Size.Width / width;
			SizeF imageSize = new SizeF(imgRef.Width, imgRef.Height);
			UIImageOrientation orient = image.Orientation;
			float boundHeight;

			switch (orient)
			{
			case UIImageOrientation.Up:                                        //EXIF = 1
				boundHeight = bounds.Size.Height;
				bounds.Size = new SizeF (boundHeight, bounds.Size.Width);
				transform = CGAffineTransform.MakeRotation (180); 
				//transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 90.0f);
				break;
				// TODO: Add other Orientations
			case UIImageOrientation.Right:                                     //EXIF = 8
				boundHeight = bounds.Size.Height;
				bounds.Size = new SizeF(boundHeight, bounds.Size.Width);
				transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0);
				transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
				break;
			default:
				throw new Exception("Invalid image orientation");                        

			}

			UIGraphics.BeginImageContext(bounds.Size);

			CGContext context = UIGraphics.GetCurrentContext();

			if (orient == UIImageOrientation.Right || orient == UIImageOrientation.Left)
			{
				context.ScaleCTM(-scaleRatio, scaleRatio);
				context.TranslateCTM(-height, 0);
			}
			else
			{
				context.ScaleCTM(scaleRatio, -scaleRatio);
				context.TranslateCTM(0, -height);
			}

			context.ConcatCTM(transform);

			context.DrawImage(new RectangleF(0, 0, width, height), imgRef);
			UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return imageCopy;
		}


		public MemoryStream CompessImage(int ratio, MemoryStream ms)
		{
			//MemoryStream compressedStream = new MemoryStream();
			//compressedStream = EZCompress1.Plugin.CrossEZCompress1.Current.compressImage(ms, 40);
			return MyCompressImage(ms,ratio) ;
		}



		public MemoryStream MyCompressImage(Stream _image, int _compressAmount)
		{
			if (_compressAmount < 1 || _compressAmount > 100)
			{
				System.Diagnostics.Debug.WriteLine("Compress amount must be between 1 and 100! Compression failed.");
				return null;
			}

			_image.Position = 0;

			using (var data = NSData.FromStream(_image))
			{
				float compressAmount = (float)(_compressAmount * .01);
				_image = null;
				var image = UIImage.LoadFromData(data);

				var newResult = image.AsJPEG(compressAmount);
				var finalStream = newResult.AsStream();

				finalStream.Position = 0;

				MemoryStream compressedStream = new MemoryStream();
				finalStream.CopyTo ( compressedStream );
				return compressedStream;
			}
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
