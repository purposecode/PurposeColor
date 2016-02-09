using System;
using PurposeColor.iOS;
using System.Runtime.CompilerServices;
using PurposeColor.interfaces;
using AlertView;
using ToastIOS;
using BigTed;

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


		public void ShowProgressbarWithCancel( string text , Action cancelAction )
		{

			BTProgressHUD.Show("cancel", cancelAction,  text,-1, ProgressHUD.MaskType.Black );
	
		}


		public void HideProgressbarWithCancel( )
		{

			BTProgressHUD.Dismiss ();

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

