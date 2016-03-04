using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PurposeColor.interfaces;
using PurposeColor.Droid.Dependency;
using Android.Graphics;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidResize))]
namespace PurposeColor.Droid.Dependency
{
    public class AndroidResize : IResize
    {
		public byte[] Resize(byte[] imageData, float width, float height,  string path)
        {
            return ResizeImageAndroid(imageData, width, height);
        }

        public MemoryStream CompessImage(int ratio, MemoryStream ms)
        {
            MemoryStream compressedStream = new MemoryStream();
			compressedStream = EZCompress1.Plugin.CrossEZCompress1.Current.compressImage(ms, ratio);
            return compressedStream;
        }


        public static byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
			Bitmap originalImage2 = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

			using (MemoryStream ms = new MemoryStream())
			{
				int streamLength = (int)ms.Length;
				int compressionRate = 100;
				#region compression ratio


				if (streamLength < 20000) {
					compressionRate = 100;
				}
				else if (streamLength < 40000) {
					compressionRate = App.screenDensity > 2 ? 100: 80;
				}
				else if (streamLength < 50000) {
					compressionRate = App.screenDensity > 2 ? 100: 70;
				}
				else if (streamLength < 100000) {
					compressionRate = App.screenDensity > 2 ? 100: 60;
				}
				else if (streamLength < 200000) {
					compressionRate = App.screenDensity > 2 ? 99: 30;
				}
				else if (streamLength <300000) {
					compressionRate = App.screenDensity > 2 ? 98: 25;
				}
				else if (streamLength < 400000) {
					compressionRate = App.screenDensity > 2 ? 97: 20;
				}
				else if (streamLength < 500000) {
					compressionRate = App.screenDensity > 2 ? 96: 15;
				}
				else {
					compressionRate = App.screenDensity > 2 ? 95: 10;
				}
				#endregion

				originalImage2.Compress(Bitmap.CompressFormat.Jpeg, compressionRate, ms);
				return ms.ToArray();
			}
        }
    }
}