using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Android.Content;
using PushNotification.Plugin;
using PushNotifictionListener;
using ImageCircle.Forms.Plugin.Droid;
using Java.IO;


namespace PurposeColor.Droid
{
    [Activity(Label = "PurposeColor", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        static Activity curentActivity;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
           
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.Forms.Forms.SetTitleBarVisibility(AndroidTitleBarVisibility.Never);
            curentActivity = this;
			CrossPushNotification.Initialize<CrossPushNotificationListener>("572461137328");
            ImageCircleRenderer.Init();

            File testFile = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.ExternalStorageDirectory.ToString() + "/PurposeColor/");
            App.DownloadsPath = testFile.AbsolutePath + "/";
			var dir =  new Java.IO.File( App.DownloadsPath  );
			if (!dir.Exists ()) {
				dir.Mkdirs ();
			} else {

				// clearing temp storage of app.
				try {
					DateTime threadStartTime = DateTime.UtcNow.AddDays(-2); // DateTime.UtcNow.AddMinutes(-60); 
					System.IO.DirectoryInfo tempFileDir = new System.IO.DirectoryInfo(testFile.AbsolutePath);

					System.IO.FileInfo[] tempFiles = tempFileDir.GetFiles();
					foreach (System.IO.FileInfo tempFile in tempFiles)
					{
						try
						{
							if (tempFile.LastAccessTime < threadStartTime)
							{
								System.IO.File.Delete(tempFile.FullName);
							}
						}
						catch (Exception ex) {
							var test = ex.Message;
						}
					}
				} catch (Exception ex) {
					var test = ex.Message;
				}
			}

            LoadApplication(new App());
        }

		protected override void OnPause ()
		{
			App.CurrentChatUserID = null;
			base.OnPause ();
		}

        public static Activity GetMainActivity()
        {
            return curentActivity;
        }
    }
}

