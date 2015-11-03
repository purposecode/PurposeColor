using System;
using Android.App;
using XamarinFormsOAuth2Demo;
using XamarinFormsOAuth2Demo.Droid;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Android.Widget;
using System.Net;
using PurposeColor;
using PurposeColor.Model;
using Java.Lang;
using System.Threading;
using PurposeColor.screens;

//[assembly: ExportRenderer(typeof(LoginGooglePage), typeof(LoginPageRenderer))]
[assembly: ExportRenderer(typeof(LoginWebViewHolder), typeof(LoginPageRenderer))]

namespace XamarinFormsOAuth2Demo.Droid
{

    public class LoginPageRenderer : PageRenderer
    {
        bool done = false;
        Activity activity = null;
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (!done)
            {
                // this is a ViewGroup - so should be able to load an AXML file and FindView<>
                activity = this.Context as Activity;
                if (App.IsGoogleLogin && !App.IsLoggedIn)
                {
                    /* ProgressDialog dlg = new ProgressDialog(Context);
                     dlg.SetTitle("Sign In To Google");
                     dlg.Show();*/

                    var myAuth = new GoogleAuthenticator("730990345527-h7r23gcdmdllgke4iud4di76b0bmpnbb.apps.googleusercontent.com",
                                     "https://www.googleapis.com/auth/userinfo.email",
                                     "https://accounts.google.com/o/oauth2/auth",
                                     "https://www.googleapis.com/plus/v1/people/me");
                    // dlg.Hide();

                    myAuth.authenticator.Completed += async (object sender, AuthenticatorCompletedEventArgs eve) =>
                    {

                        /* ProgressDialog dlg2 = new ProgressDialog(Context);
                         dlg2.SetTitle("Sign In To Google");
                         dlg2.Show();*/
                        if (eve.IsAuthenticated)
                        {
                            var user = await myAuth.GetProfileInfoFromGoogle(eve.Account.Properties["access_token"].ToString());
                            //  PurposeColor.App.NavigateToChangePassword( user);
                            App.IsLoggedIn = true;
                        }
                        //  dlg2.Hide();
                    };

                    //auth.AllowCancel = false;
                    activity.StartActivity(myAuth.authenticator.GetUI(activity));
                    done = true;
                }
                else if (App.IsFacebookLogin && !App.IsLoggedIn)
                {
                    var auth = new OAuth2Authenticator(
                        clientId: "1218463084847397", // your OAuth2 client id
                        scope: "", // the scopes for the particular API you're accessing, delimited by "+" symbols
                        authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),//new Uri(""), // the auth URL for the service
                        redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html")); // the redirect URL for the service

                    auth.Completed += (sender, eventArgs) =>
                    {
                        if (eventArgs.IsAuthenticated)
                        {
                            App.SuccessfulLoginAction.Invoke();
                            
                            try
                            {
                                App.IsLoggedIn = true;
                                App.SaveToken(eventArgs.Account.Properties["access_token"]);
                                AccountStore.Create(activity).Save(eventArgs.Account, "Facebook");
                                // save user token  - to local DB
                            }
                            catch (System.Exception ex)
                            {
                                Console.WriteLine("auth.Completed ::: " + ex.Message);
                            }
                            

                            var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, eventArgs.Account);
                            request.GetResponseAsync().ContinueWith(t =>
                            {
                                if (t.IsFaulted)
                                    Console.WriteLine("Error: " + t.Exception.InnerException.Message);
                                else
                                {
                                    string json = t.Result.GetResponseText();
                                    Console.WriteLine(json);

                                    // save user details for profile page - to local DB
                                }
                            });
                        }
                        else
                        {
                            // The user cancelled
                        }
                    };
                    activity.StartActivity(auth.GetUI(activity));
                } // end - else if
            } //end - if (!done)
        }// end - OnElementChanged()
    }
}
