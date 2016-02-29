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
using PurposeColor.screens;
using Newtonsoft.Json;
using PurposeColor.Model;


[assembly: ExportRenderer(typeof(LoginWebViewHolder), typeof(LoginPageRenderer))]
namespace PurposeColor.iOS
{
    public class LoginPageRenderer : PageRenderer
    {
        DialogViewController dialog;
        UIWindow window;
        public LoginPageRenderer()
        {
            dialog = new DialogViewController(new RootElement("Sample"));

            window = new UIWindow(UIScreen.MainScreen.Bounds);
            window.RootViewController = new UINavigationController(dialog);
            window.MakeKeyAndVisible();

            if (App.IsGoogleLogin && !App.IsLoggedIn)
            {
                var myAuth = new GoogleAuthenticator("730990345527-h7r23gcdmdllgke4iud4di76b0bmpnbb.apps.googleusercontent.com",
                    "https://www.googleapis.com/auth/userinfo.email",
                    "https://accounts.google.com/o/oauth2/auth",
                    "https://www.googleapis.com/plus/v1/people/me");

                myAuth.authenticator.Completed += async (object sender, AuthenticatorCompletedEventArgs eve) =>
                {
                    dialog.DismissViewController(true, null);
                    window.Hidden = true;
                    dialog.Dispose();
                    window.Dispose();
                    if (eve.IsAuthenticated)
                    {
                        var user = await myAuth.GetProfileInfoFromGoogle(eve.Account.Properties["access_token"].ToString());
						await App.SaveUserData(user,true);
                    }
                };

                UIViewController vc = myAuth.authenticator.GetUI();
                dialog.PresentViewController(vc, true, null);
            }
        }

		async void SerialiseFacebookUserData(string json)
		{
			FacebookInfo fbUser = JsonConvert.DeserializeObject<FacebookInfo>(json);
			User user = new User ();
			user.UserName = fbUser.name;
			user.DisplayName = fbUser.name;
			user.Email = fbUser.email;
			user.UserId = fbUser.id;

			await PurposeColor.App.SaveUserData( user, false);
		}

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (App.IsFacebookLogin && !App.IsLoggedIn)
            {
				
				try {
					var auth = new OAuth2Authenticator (
									clientId: "1064717540213918", //App OAuth2 client id
						           scope: "", // the scopes for the particular API you're accessing, delimited by "+" symbols
						           authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),//new Uri(""), // the auth URL for the service
						           redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html")); // the redirect URL for the service
					
					auth.Completed += (sender, eventArgs) => {
						if (eventArgs.IsAuthenticated) {
							App.SuccessfulLoginAction.Invoke ();
							App.SaveToken (eventArgs.Account.Properties ["access_token"]);
					
							var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me"), null, eventArgs.Account);
							request.GetResponseAsync ().ContinueWith (t => {
								if (t.IsFaulted)
									Console.WriteLine ("Error: " + t.Exception.InnerException.Message);
								else {
									string json = t.Result.GetResponseText ();
									Console.WriteLine (json);
									SerialiseFacebookUserData (json);
								}
							});
						}
					};

					dialog.PresentViewController (auth.GetUI (), true, null);
				}
				catch (Exception ex) 
				{
					Console.WriteLine ("ViewDidAppear :: " + ex.Message);
				}


            }
        }
    }

	class FacebookInfo
	{
		public string id { get; set; }
		public string email { get; set; }
		public string name { get; set; }
	}
}