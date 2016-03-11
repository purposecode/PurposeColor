using System;
using PurposeColor.iOS;
using System.IO;
using Foundation;
using MediaPlayer;
using UIKit;
using AVFoundation;
using CoreGraphics;
using Xamarin.Forms;
using CoreMedia;


[assembly: Xamarin.Forms.Dependency(typeof(IOVVideoCompression))]
namespace PurposeColor.iOS
{
	public class IOVVideoCompression : IVideoCompressor
	{
		public IOVVideoCompression ()
		{
		}

		public MemoryStream CompressVideo( string sourceFilePath, string destinationFilePath, bool deleteSourceFile )
		{
			return null;
		}

		public void CreateVideoThumbnail ( string inputVideoPath, string outputImagePath )
		{

			try 
			{
				string downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				string downloadFilePath = Path.Combine(downloadPath, "first_video_thump.jpg");

				/*NSUrl videourl = new NSUrl ( inputVideoPath );
				MPMoviePlayerController moviePlayer = new MPMoviePlayerController ( videourl );
				moviePlayer.ShouldAutoplay = false;
				UIImage videoThumb = moviePlayer.ThumbnailImageAt (0.002, MPMovieTimeOption.Exact);
				NSData videoData = videoThumb.AsJPEG ();
				videoData.Save ( downloadFilePath, true );	*/

				UIImage test = GetVideoThumbnail( inputVideoPath );
				NSData videoData = test.AsJPEG ();
				videoData.Save ( downloadFilePath, false );
				string com = "com";



			} 
			catch (Exception ex) 
			{
				System.Diagnostics.Debug.WriteLine ( ex.Message );
			}

		}

		private UIImage GetVideoThumbnail(string path)
		{
			try 
			{
				CMTime actualTime;
				NSError outError;
				using (var asset = AVAsset.FromUrl (NSUrl.FromFilename (path)))
				using (var imageGen = new AVAssetImageGenerator (asset))
				using (var imageRef = imageGen.CopyCGImageAtTime (new CMTime (1, 1), out actualTime, out outError)) 
				{
					return UIImage.FromImage (imageRef);
				}   
			} 
			catch
			{
				return null;
			}
		}
	}
}

