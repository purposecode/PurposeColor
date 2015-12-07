using System;
using PurposeColor.iOS;
using System.Runtime.CompilerServices;
using PurposeColor.interfaces;
using AlertView;
using ToastIOS;

[assembly: Xamarin.Forms.Dependency(typeof(ProgressBarImpl))]
namespace PurposeColor.iOS
{
	public class ProgressBarImpl: IProgressBar
	{


		public void ShowProgressbar(string text)
		{
			MBHUDView.HudWithBody (
				body: text,
				aType: MBAlertViewHUDType.ActivityIndicator, 
				delay: 120, 
				showNow: true
			);
		}


		public void HideProgressbar()
		{
			MBHUDView.DismissCurrentHUD ();
		}

		public void ShowToast(string messege)
		{
			Toast.MakeText(messege).Show(ToastType.None);
		}
	}
}

