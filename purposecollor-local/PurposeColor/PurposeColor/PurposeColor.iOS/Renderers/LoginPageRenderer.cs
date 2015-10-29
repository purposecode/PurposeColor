using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using PurposeColor;
using PurposeColor.iOS;
using XamarinFormsOAuth2Demo.iOS;
using Xamarin.Auth;
using UIKit;
using MonoTouch;
using MonoTouch.Dialog;


[assembly: ExportRenderer(typeof(LoginGooglePage), typeof(LoginPageRenderer))]
namespace PurposeColor.iOS
{
	public class LoginPageRenderer : PageRenderer
	{
		DialogViewController dialog;
		UIWindow window;
		public LoginPageRenderer ()
		{
			 dialog = new DialogViewController( new RootElement( "Sample" ) );

		    window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = new UINavigationController (dialog);
			window.MakeKeyAndVisible ();


			var myAuth = new GoogleAuthenticator ("730990345527-h7r23gcdmdllgke4iud4di76b0bmpnbb.apps.googleusercontent.com",
				"https://www.googleapis.com/auth/userinfo.email",
				"https://accounts.google.com/o/oauth2/auth",
				"https://www.googleapis.com/plus/v1/people/me");

		

			myAuth.authenticator.Completed += async (object sender, AuthenticatorCompletedEventArgs eve) => 
			{
				dialog.DismissViewController(true, null);
				window.Hidden = true;
				dialog.Dispose();
				window.Dispose();
				if( eve.IsAuthenticated )
				{
					var user = await myAuth.GetProfileInfoFromGoogle( eve.Account.Properties["access_token"].ToString());
				}
			};
				

			UIViewController vc = myAuth.authenticator.GetUI ();
			dialog.PresentViewController (vc, true, null);

		}
	}
}

