using System;
using System.Net;
using System.IO;
using UIKit;
using PurposeColor.iOS.Dependency;
using PurposeColor.iOS;
using AssetsLibrary;
using System.Threading.Tasks;
using AlertView;
using MediaPlayer;
using Foundation;
using System.Linq;


[assembly: Xamarin.Forms.Dependency(typeof(IOSVideoDownloader))]
namespace PurposeColor.iOS
{
	public class IOSVideoDownloader : IVideoDownloader
	{
		public IOSVideoDownloader ()
		{
		}



		public void PlayVideo (string path)
		{
			MPMoviePlayerController moviePlayer; 
			moviePlayer = new MPMoviePlayerController (NSUrl.FromFilename (path));
			UIApplication.SharedApplication.KeyWindow.RootViewController.Add( moviePlayer.View );
			moviePlayer.ShouldAutoplay = true;
			moviePlayer.SetFullscreen (true,true);
			moviePlayer.Play ();
		}


		public async Task<bool> Download(string uri, string filename)
		{
			try
			{
				MBHUDView.HudWithBody (
					body: "Downloading video....",
					aType: MBAlertViewHUDType.ActivityIndicator, 
					delay: 120, 
					showNow: true
				);

				var webClient = new WebClient();

				webClient.DownloadDataCompleted += async (s, e) =>
				{

					var bytes = e.Result; // get the downloaded data
					string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					string localFilename = filename;
					string localPath = Path.Combine(documentsPath, localFilename);
					File.WriteAllBytes(localPath, bytes); // writes to local storage   

					ALAssetsLibrary videoLibrary = new ALAssetsLibrary();
					await videoLibrary.WriteVideoToSavedPhotosAlbumAsync( new Foundation.NSUrl( localPath ));

					MBHUDView.DismissCurrentHUD();


					PlayVideo( localPath );



				};

				var url = new Uri(uri);

				webClient.DownloadDataAsync(url);
				return true;
			}
			catch( Exception ex ) 
			{
				return false;
			}

		}

		void OnDownloadVideoCompleted (object sender, DownloadDataCompletedEventArgs e)
		{
			
		}
	}
}


