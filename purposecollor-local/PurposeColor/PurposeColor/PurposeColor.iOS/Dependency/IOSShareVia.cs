using System;
using PurposeColor.interfaces;
using PurposeColor.iOS;
using UIKit;
using Foundation;
using System.Linq;
using System.Diagnostics;
using ImageIO;

[assembly: Xamarin.Forms.Dependency(typeof(IOSShareVia))]
namespace PurposeColor.iOS
{
	public class IOSShareVia : IShareVia
	{
		ProgressBarImpl progress = new ProgressBarImpl();
		public IOSShareVia()
		{
		}

		public void ShareMedia( string text, string path, PurposeColor.Constants.MediaType type )
		{
			try 
			{
				
				//progress.ShowProgressbar( "Preparing sharing options...." );

				NSMutableArray sharingItems = new NSMutableArray ();


				NSString testdata = new NSString(  text );

				NSObject[] shareArray = new NSObject[2];
				shareArray [0] = testdata;

				if (type == Constants.MediaType.Image  && !string.IsNullOrEmpty( path ))  
				{

					UIImage image = UIImage.LoadFromData( NSData.FromUrl( new NSUrl(path) ) );
					shareArray [1] = image;
				}
				else if( type == Constants.MediaType.Video && !string.IsNullOrEmpty( path ) )
				{
					NSString videoUrl = new NSString( path );
					shareArray[1] = videoUrl;
				}

				var firstController = UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers.First().ChildViewControllers.Last().ChildViewControllers.First();

				UIActivityViewController controller = new UIActivityViewController ( shareArray, null  );

				firstController.PresentViewController ( controller, true, Controlerloaded);
		
			}
			catch (Exception ex) 
			{
				Debug.WriteLine ( ex.Message );					
			}

		}


		private void Controlerloaded()
		{
			progress.HideProgressbar ();
		}
	}
}

