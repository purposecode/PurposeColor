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
using AVFoundation;
using MonoTouch;


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
			
			try 
			{
				string downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				string fileName = Path.GetFileNameWithoutExtension( sourceFilePath ) + ".mp4";
				string downloadFilePath = Path.Combine(downloadPath, fileName );

				var asset = AVAsset.FromUrl( NSUrl.FromFilename( sourceFilePath ) );

				AVAssetExportSession export = new AVAssetExportSession (asset, AVAssetExportSession.PresetLowQuality );

				export.OutputUrl = NSUrl.FromFilename( downloadFilePath );
				export.OutputFileType = AVFileType.Mpeg4;
				export.ShouldOptimizeForNetworkUse = true;

				export.ExportTaskAsync().Wait();

				MemoryStream ms = new MemoryStream();    
				FileStream file = new FileStream(  downloadFilePath, FileMode.Open, FileAccess.Read);
				file.CopyTo ( ms );
				file.Close();
				return ms;

			} 
			catch (Exception ex) 
			{
				System.Diagnostics.Debug.WriteLine ( ex.Message );
				return null;
			}

		
		}


		public Xamarin.Forms.Size GetCameraSize()
		{
			Xamarin.Forms.Size camSize = new  Xamarin.Forms.Size ();
			return camSize;
		}

		public MemoryStream CreateVideoThumbnail ( string inputVideoPath, string outputImagePath )
		{

			try 
			{
				string downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				string fileName = Path.GetFileNameWithoutExtension( inputVideoPath ) + ".jpg";
				string downloadFilePath = Path.Combine(downloadPath, fileName );
				 
				UIImage thumbImage = GetVideoThumbnail( inputVideoPath );

				/*if( thumbImage.Orientation == UIImageOrientation.Up )
				{
					UIImage rotatedImage = UIImage.FromImage(  thumbImage.CGImage, thumbImage.CurrentScale, UIImageOrientation.Right );
					NSData videoData = rotatedImage.AsJPEG ();
					videoData.Save ( downloadFilePath, false );
				}
				else
				{
					NSData videoData = thumbImage.AsJPEG ();
					videoData.Save ( downloadFilePath, false );
				}*/

				NSData videoData = thumbImage.AsJPEG ();
				videoData.Save ( downloadFilePath, false );


				MemoryStream ms = new MemoryStream();    
				FileStream file = new FileStream(  downloadFilePath, FileMode.Open, FileAccess.Read);
				file.CopyTo ( ms );
				file.Close();
				return ms;

			} 
			catch (Exception ex) 
			{
				System.Diagnostics.Debug.WriteLine ( ex.Message );
				return null;
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
				{
					imageGen.AppliesPreferredTrackTransform = true;
					using (var imageRef = imageGen.CopyCGImageAtTime (new CMTime (1, 1), out actualTime, out outError)) 
					{
						return UIImage.FromImage (imageRef);
					}
				}
   
			} 
			catch
			{
				return null;
			}
		}
	}
}

