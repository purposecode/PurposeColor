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
		public IOSShareVia()
		{
		}

		public void ShareMedia( string text, string path, PurposeColor.Constants.MediaType type )
		{
			try 
			{
				//MonoTouch.ObjCRuntime.Class.ThrowOnInitFailure

				NSMutableArray sharingItems = new NSMutableArray ();


				NSString testdata = new NSString(  text );

				NSObject[] shareArray = new NSObject[2];
				shareArray [0] = testdata;

				if (type == Constants.MediaType.Image  && !string.IsNullOrEmpty( path ))  
				{

					UIImage image = UIImage.LoadFromData( NSData.FromUrl( new NSUrl(path) ) );
					shareArray [1] = image;
				}

				var firstController = UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers.First().ChildViewControllers.Last().ChildViewControllers.First();

				UIActivityViewController controller = new UIActivityViewController ( shareArray, null  );

				firstController.PresentViewController ( controller, true, null );
			}
			catch (Exception ex) 
			{
				Debug.WriteLine ( ex.Message );					
			}

		}
	}
}

