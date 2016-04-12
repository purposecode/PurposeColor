using System;
using PurposeColor.Droid;
using Xamarin.Geolocation;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Media.Plugin.Abstractions;
using Android.Graphics;
using System.IO;
using Android.Media;
using System;
using System.Threading.Tasks;
using Android.Media;
using MediaOrientation = Android.Media.Orientation;
using System.IO;
using Android.Graphics;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidMediaView))]
namespace PurposeColor.Droid
{
	public class AndroidMediaView : IMediaVIew
	{
		public AndroidMediaView ()
		{
		}

		public void ShowImage (string imageURL)
		{
			Intent intent = new Intent();
			intent.AddFlags (ActivityFlags.NewTask);
			intent.SetAction(Intent.ActionView);
			intent.SetDataAndType(Android.Net.Uri.Parse("file://" + imageURL), "image/*");
			string fullPath = System.IO.Path.GetDirectoryName( imageURL );
			//intent.SetDataAndType(Android.Net.Uri.Parse("content://" + fullPath), "image/*");
			Application.Context.StartActivity ( intent );
			//Application.Context.StartActivity( new Intent( Intent.ActionView, Android.Net.Uri.Parse("content://" + fullPath + "/" )));
		}


		async public Task<bool> FixOrientationAsync(MediaFile file)
		{
			if (file == null)
				return false;
			try
			{

				var filePath = file.Path;
				var orientation = GetRotation(filePath);

				if (!orientation.HasValue)
					return false;

				Bitmap bmp = RotateImage(filePath, orientation.Value);
				var quality = 90;

				using (var stream = File.Open(filePath, FileMode.OpenOrCreate))
					await bmp.CompressAsync(Bitmap.CompressFormat.Png, quality, stream);

				bmp.Recycle();

				return true;
			}
			catch (Exception ex)
			{                               
				//ex.Report(); //Extension method for Xamarin.Insights
				return false;
			}
		}

		static int? GetRotation(string filePath)
		{
			try
			{
				ExifInterface ei = new ExifInterface(filePath);
				var orientation = (MediaOrientation)ei.GetAttributeInt(ExifInterface.TagOrientation, (int)MediaOrientation.Normal);
				switch (orientation)
				{
				case MediaOrientation.Rotate90:
					return 90;
				case MediaOrientation.Rotate180:
					return 180;
				case MediaOrientation.Rotate270:
					return 270;
				default:
					return null;
				}

			}
			catch (Exception ex)
			{
				//ex.Report();
				return null;
			}
		}

		private static Bitmap RotateImage(string filePath, int rotation)
		{
			Bitmap originalImage = BitmapFactory.DecodeFile(filePath);

			Matrix matrix = new Matrix();
			matrix.PostRotate(rotation);
			var rotatedImage = Bitmap.CreateBitmap(originalImage, 0, 0, originalImage.Width, originalImage.Height, matrix, true);
			originalImage.Recycle();
			return rotatedImage;
		}
	}
}

