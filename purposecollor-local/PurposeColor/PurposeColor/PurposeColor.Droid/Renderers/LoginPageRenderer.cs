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

[assembly: ExportRenderer(typeof(LoginGooglePage), typeof(LoginPageRenderer))]

namespace XamarinFormsOAuth2Demo.Droid
{



	public class LoginPageRenderer : PageRenderer
	{
		bool done = false;

        protected override async void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);

			if (!done)
            {

				// this is a ViewGroup - so should be able to load an AXML file and FindView<>
				var activity = this.Context as Activity;


                
                   /* ProgressDialog dlg = new ProgressDialog(Context);
                    dlg.SetTitle("Sign In To Google");
                    dlg.Show();*/

				var myAuth = new GoogleAuthenticator ("730990345527-h7r23gcdmdllgke4iud4di76b0bmpnbb.apps.googleusercontent.com",
					             "https://www.googleapis.com/auth/userinfo.email",
					             "https://accounts.google.com/o/oauth2/auth",
					             "https://www.googleapis.com/plus/v1/people/me");
               // dlg.Hide();

				myAuth.authenticator.Completed += async (object sender, AuthenticatorCompletedEventArgs eve) => 
				{

                   /* ProgressDialog dlg2 = new ProgressDialog(Context);
                    dlg2.SetTitle("Sign In To Google");
                    dlg2.Show();*/
					if( eve.IsAuthenticated )
					{
						 var user = await myAuth.GetProfileInfoFromGoogle( eve.Account.Properties["access_token"].ToString());
                       //  PurposeColor.App.NavigateToChangePassword( user);
					}
                  //  dlg2.Hide();
				};

              

              
				//auth.AllowCancel = false;
				activity.StartActivity (myAuth.authenticator.GetUI (activity));
				done = true;
			}
		}


	}
}
