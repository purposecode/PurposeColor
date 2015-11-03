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
                    }
                };

                UIViewController vc = myAuth.authenticator.GetUI();
                dialog.PresentViewController(vc, true, null);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (App.IsFacebookLogin && !App.IsLoggedIn)
            {
                var auth = new OAuth2Authenticator(
                clientId: "1218463084847397",
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

                auth.Completed += (sender, eventArgs) =>
                {
                    if (eventArgs.IsAuthenticated)
                    {
                        App.SuccessfulLoginAction.Invoke();
                        App.SaveToken(eventArgs.Account.Properties["access_token"]);

                        AccountStore.Create().Save(eventArgs.Account, "Facebook");

                        var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, eventArgs.Account);
                        request.GetResponseAsync().ContinueWith(t =>
                        {
                            if (t.IsFaulted)
                                Console.WriteLine("Error: " + t.Exception.InnerException.Message);
                            else
                            {
                                string json = t.Result.GetResponseText();
                                Console.WriteLine(json);
                            }
                        });
                    }
                    else
                    {
                        // The user cancelled
                    }
                };

                PresentViewController(auth.GetUI(), true, null);
            }
        }
    }
}

