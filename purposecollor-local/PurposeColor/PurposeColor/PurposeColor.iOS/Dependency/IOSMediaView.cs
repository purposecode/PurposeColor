using System;
using PurposeColor.iOS;
using System.Threading.Tasks;
using XLabs.Platform.Services.Media;
using Foundation;
using UIKit;
using Media.Plugin;
using System.Linq;
using System.Drawing;
using Xamarin.Forms;


[assembly: Xamarin.Forms.Dependency(typeof(IOSMediaView))]
namespace PurposeColor.iOS
{

	public class DocInteractionC : UIDocumentInteractionControllerDelegate
	{
		readonly UINavigationController _navigationController;

		public DocInteractionC(UINavigationController controller)
		{
			_navigationController = controller;
		}

		public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
		{

			return _navigationController;

		} 

		public override UIView ViewForPreview(UIDocumentInteractionController controller)
		{

			return _navigationController.View;

		}

	}


	public class IOSMediaView : IMediaVIew
	{
		public IOSMediaView ()
		{
		}


		public async Task<bool> FixOrientationAsync(Media.Plugin.Abstractions.MediaFile file)
		{
			await Task.Delay ( TimeSpan.FromSeconds(1) );
			var val = true;
			return val;
		}

		public void ShowImage (string imageURL)
		{

			Xamarin.Forms.Device.BeginInvokeOnMainThread(() => 
			{

				var firstController = UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers.First().ChildViewControllers.Last().ChildViewControllers.First();

				var navcontroller = firstController as UINavigationController;

				var uidic = UIDocumentInteractionController.FromUrl(new NSUrl(imageURL, true));

				navcontroller.HidesBarsOnSwipe = false;
				navcontroller.HidesBarsOnTap = false;
				navcontroller.HidesBarsWhenVerticallyCompact = false;
				navcontroller.NavigationBarHidden = false;

				uidic.Delegate = new DocInteractionC(navcontroller); 

				uidic.PresentPreview(true);

			});

		}


	}
}

