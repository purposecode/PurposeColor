using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using googleforms;
using OAuth_Demo;
using Newtonsoft.Json;
using System.Net;
using System.Windows;
using Xamarin.Forms;
using googleforms.WinPhone;
using PurposeColor.interfaces;
using Microsoft.Phone.Shell;

[assembly: Dependency(typeof(AutheticateImpl))]
namespace googleforms.WinPhone
{

    class GoogleInfo
    {
        public string id { get; set; }
        public string email { get; set; }
        public bool verified_email { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string link { get; set; }
        public string picture { get; set; }
        public string gender { get; set; }
    }


    public class AutheticateImpl : IAuthenticate
    {

        private void SetProgressIndicator(bool value)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = value;
            SystemTray.ProgressIndicator.IsVisible = value;
        }


        public async void AutheticateGoogle()
        {

            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "Acquiring";

            SetProgressIndicator(true);


            if (!PurposeColor.App.loggedin)
            {
                OAuthAuthorization authorization = new OAuthAuthorization(
                "https://accounts.google.com/o/oauth2/auth",
                "https://accounts.google.com/o/oauth2/token");
                TokenPair tokenPair = await authorization.Authorize(
                    "394204572562-3t7po22vtj0vuug7ungao7k0ih7e7i6l.apps.googleusercontent.com",
                    "h0vT8QqVogLluvfL_8exk0hH",
                    new string[] { GoogleScopes.UserinfoEmail });


                GetProfileInfoFromGoogle(tokenPair.AccessToken);



                // Request a new access token using the refresh token (when the access token was expired)
                TokenPair refreshTokenPair = await authorization.RefreshAccessToken(
                    "394204572562-3t7po22vtj0vuug7ungao7k0ih7e7i6l.apps.googleusercontent.com",
                    "h0vT8QqVogLluvfL_8exk0hH",
                    tokenPair.RefreshToken);
            }

            SetProgressIndicator(false);
        }


        const string googUesrInfoAccessleUrl = "https://www.googleapis.com/oauth2/v1/userinfo?access_token={0}";
        GoogleInfo googleInfo;
        public async Task<bool> GetProfileInfoFromGoogle(string access_token)
        {
            bool isValid = false;
            //Google API REST request
            string userInfo = await fnDownloadString(string.Format(googUesrInfoAccessleUrl, access_token));
            if (userInfo != "Exception")
            {
                //step 4: Deserialize the JSON response to get data in class object
                googleInfo = JsonConvert.DeserializeObject<GoogleInfo>(userInfo);
                isValid = true;
            }
            else
            {

                isValid = false;
                //Toast.MakeText (Context, "connrection failed", ToastLength.Short);
                //	Toast.MakeText(this, "Connection failed! Please try again", ToastLength.Short).Show();
            }
            /*if (progress != null)
            {
                progress.Dismiss();
                progress = null;
            }*/
            return isValid;
        }

        async Task<string> fnDownloadString(string strUri)
        {
            var webclient = new WebClient();
            string strResultData = "";
            try
            {

                //  strResultData = await webclient.DownloadStringTaskAsync(new Uri(strUri));
                webclient.DownloadStringAsync(new Uri(strUri));
                webclient.DownloadStringCompleted += webclient_DownloadStringCompleted;
                //    Console.WriteLine(strResultData);
            }
            catch
            {
                strResultData = "Exception";
                /*	RunOnUiThread(() =>
                    Toast.MakeText(this, "Unable to connect to server!!!", ToastLength.Short).Show());*/
            }
            finally
            {
                if (webclient != null)
                {
                    //  webclient.Dispose();
                    webclient = null;
                }
            }
            return strResultData;
        }

        void webclient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            googleInfo = JsonConvert.DeserializeObject<GoogleInfo>(e.Result);

            MessageBox.Show(googleInfo.email + " :: " + googleInfo.given_name + " :: " + googleInfo.name);
            //   MessageDialog msgbox = new MessageDialog("Message Box is displayed"); 

            string test = "test";
        }


    }
}
