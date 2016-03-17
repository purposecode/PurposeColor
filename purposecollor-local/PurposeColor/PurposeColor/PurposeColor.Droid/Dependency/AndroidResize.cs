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
			long streamLength = (long)originalImage.ByteCount;
			using (MemoryStream ms = new MemoryStream())
			{
				int compressionRate = 100;

				#region compression ratio
				if (streamLength < 20000) {
					compressionRate = 100;
				}
				else if (streamLength < 40000) {
					compressionRate = App.screenDensity > 2 ? 100: 95;
				}
				else if (streamLength < 50000) {
					compressionRate = App.screenDensity > 2 ? 100: 91;
				}
				else if (streamLength < 100000) {
					compressionRate = App.screenDensity > 2 ? 100: 90;
				}
				else if (streamLength < 200000) {
					compressionRate = App.screenDensity > 2 ? 99: 89;
				}
				else if (streamLength <300000) {
					compressionRate = App.screenDensity > 2 ? 98: 88;
				}
				else if (streamLength < 400000) {
					compressionRate = App.screenDensity > 2 ? 97: 87;
				}
				else if (streamLength < 500000) {
					compressionRate = App.screenDensity > 2 ? 96: 86;
				}
				else if (streamLength < 600000) {
					compressionRate = App.screenDensity > 2 ? 95: 86;
				}
				else if (streamLength < 700000) {
					compressionRate = App.screenDensity > 2 ? 94: 86;
				}
				else if (streamLength < 900000) {
					compressionRate = App.screenDensity > 2 ? 92: 86;
				}
				else if (streamLength < 1000000) {
					compressionRate = App.screenDensity > 2 ? 90: 86;
				}
				else if (streamLength < 2000000) {
					compressionRate = App.screenDensity > 2 ? 89: 86;
				}
				else {
					compressionRate = App.screenDensity > 2 ? 88: 85;
				}
				#endregion

				originalImage2.Compress(Bitmap.CompressFormat.Jpeg, compressionRate, ms);
				return ms.ToArray();
			}
		}
    }
}