using System;
using MediaPlayer;
using UIKit;
using System.Runtime.CompilerServices;
using videoplayer.iOS;
using Foundation;
using System.Linq;
using PurposeColor;

[assembly: Xamarin.Forms.Dependency(typeof(IOSVideoPlayer))] 
namespace videoplayer.iOS
{
	public class IOSVideoPlayer : IPlayVideo
	{
		public void playVid()
		{
			MPMoviePlayerController moviePlayer; 
			moviePlayer = new MPMoviePlayerController (NSUrl.FromFilename ( App.SelectedVideoPath ));
			//UIApplication.SharedApplication.Window.RootController.Add(moviePlayer.View);
			//var firstController = UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers.First().ChildViewControllers.Last().ChildViewControllers.First(); 
			UIApplication.SharedApplication.KeyWindow.RootViewController.Add( moviePlayer.View );

			moviePlayer.ShouldAutoplay = true;
			moviePlayer.SetFullscreen (true,true);
			moviePlayer.Play ();
			//firstController.Add ( moviePlayer.View );
		}
	}
}

