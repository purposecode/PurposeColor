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
        public byte[] Resize(byte[] imageData, float width, float height)
        {
            return ResizeImageAndroid(imageData, width, height);
        }

        public MemoryStream CompessImage(int ratio, MemoryStream ms)
        {
            MemoryStream compressedStream = new MemoryStream();
            compressedStream = EZCompress1.Plugin.CrossEZCompress1.Current.compressImage(ms, 50);
            return compressedStream;
        }


        public static byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}