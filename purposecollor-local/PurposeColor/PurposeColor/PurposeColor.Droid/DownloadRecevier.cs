using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Java.IO;
using Xamarin.Forms;
using Android.Content.PM;
using System.IO;

namespace PurposeColor.Droid
{
   
    [BroadcastReceiver(Enabled = true, Label = "MyTestReceiver")]
    [IntentFilter(new[] { DownloadManager.ActionDownloadComplete })]
    [Activity(Label = "MyTestReceiver", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MyTestReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                AndHUD.Shared.Dismiss();

                Android.App.DownloadManager dm = (Android.App.DownloadManager)Xamarin.Forms.Forms.Context.GetSystemService(Android.Content.Context.DownloadService);

                Android.Net.Uri downloadedFileUri = dm.GetUriForDownloadedFile(App.DownloadID);


                if( App.ShareDownloadID != 0 )
                {
                    downloadedFileUri = dm.GetUriForDownloadedFile(App.ShareDownloadID);
                    MessagingCenter.Send<MyTestReceiver, string>(this, "boom", downloadedFileUri.ToString());
                    App.ShareDownloadID = 0;
                    string filePath = downloadedFileUri.ToString().Remove(0, 7);
                    App.DownloadsPath =  Path.GetDirectoryName(filePath);
                    return;
                }

          

                if( downloadedFileUri != null )
                {
                    App.DownloadsPath = downloadedFileUri.ToString().Remove(0, 7);
                    Java.IO.File file = new Java.IO.File(new Java.Net.URI(downloadedFileUri.ToString()));
                    Intent videoPlayerActivity = new Intent(Intent.ActionView);
                    videoPlayerActivity.SetDataAndType(Android.Net.Uri.FromFile(file), "video/*");
                    Activity activity = Forms.Context as Activity;
                    activity.StartActivity(videoPlayerActivity);
                }
                App.DownloadID = 0;

            }
            catch (Exception ex)
            {
                
                throw;
            }

        }
    }
}