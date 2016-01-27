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


                MessagingCenter.Send<MyTestReceiver, DateTime>(this, "boom", DateTime.Now);

                if( downloadedFileUri != null )
                {
                    File file = new File(new Java.Net.URI(downloadedFileUri.ToString()));
                    Intent videoPlayerActivity = new Intent(Intent.ActionView);
                    videoPlayerActivity.SetDataAndType(Android.Net.Uri.FromFile(file), "video/*");
                    Activity activity = Forms.Context as Activity;
                    activity.StartActivity(videoPlayerActivity);
                }

            }
            catch (Exception ex)
            {
                
                throw;
            }

        }
    }
}