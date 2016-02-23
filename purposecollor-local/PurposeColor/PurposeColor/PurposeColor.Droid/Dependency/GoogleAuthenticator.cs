using System;
using Android.App;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Android.Widget;
using System.Net;
using Xamarin.Auth;
using PurposeColor.Model;

namespace XamarinFormsOAuth2Demo.Droid
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

	public class GoogleAuthenticator
	{
		public string ClientId{ get; set; }
		public string Scope{ get; set; }
		public string  AuthorizeUrl{ get; set; }
		public string  RedirectUrl{ get; set; }
        public string LoggedInUserName { get; set; }
		GoogleInfo googleInfo;
		ProgressDialog progress;
		public OAuth2Authenticator authenticator;

		public GoogleAuthenticator ( string clientID, string scope, string authUrl, string redirectUrl )
		{
			ClientId = clientID;
			Scope = scope;
			AuthorizeUrl = authUrl;
			RedirectUrl = redirectUrl;
			authenticator = new OAuth2Authenticator (ClientId, Scope, new Uri( AuthorizeUrl), new Uri(RedirectUrl));
		}

		const string googUesrInfoAccessleUrl = "https://www.googleapis.com/oauth2/v1/userinfo?access_token={0}";
		public async Task<User> GetProfileInfoFromGoogle(string access_token)
		{
			User user = new User();
			//Google API REST request
			string userInfo = await fnDownloadString(string.Format(googUesrInfoAccessleUrl, access_token));
			if (userInfo != "Exception")
			{
				//step 4: Deserialize the JSON response to get data in class object
				googleInfo = JsonConvert.DeserializeObject<GoogleInfo>(userInfo);
                LoggedInUserName = googleInfo.email;
			}
			else
			{
				if (progress != null)
				{
					progress.Dismiss();
					progress = null;
				}
				//Toast.MakeText (Context, "connrection failed", ToastLength.Short);
				//	Toast.MakeText(this, "Connection failed! Please try again", ToastLength.Short).Show();
			}
			/*if (progress != null)
			{
				progress.Dismiss();
				progress = null;
			}*/
			user.UserName = googleInfo.name;
			user.DisplayName = googleInfo.name;
			user.AllowCommunitySharing = true;
			user.AuthenticationToken = access_token;
			user.Email = googleInfo.email;
			user.Gender = googleInfo.gender;
			user.ProfileImageUrl = googleInfo.picture;
				
            return user;
		}

		async Task<string> fnDownloadString(string strUri)
		{
			var webclient = new WebClient();
			string strResultData;
			try
			{
				strResultData = await webclient.DownloadStringTaskAsync(new Uri(strUri));
				Console.WriteLine(strResultData);
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
					webclient.Dispose();
					webclient = null;
				}
			}
			return strResultData;
		}
	}
}

