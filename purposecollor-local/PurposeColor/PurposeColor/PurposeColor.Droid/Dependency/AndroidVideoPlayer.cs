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
using System.Threading.Tasks;
using PurposeColor.Droid.Dependency;
using Java.IO;
using Xamarin.Forms;
using Android.Database;
using AndroidHUD;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidVideoPlayer))]
namespace PurposeColor.Droid.Dependency
{
    class AndroidVideoPlayer : IVideoDownloader
    {


        public async Task<bool> Download(string uri, string filename)
        {

            string downloadedFolder = "/storage/emulated/0/Download/";
            if (System.IO.File.Exists(downloadedFolder +"/" + filename))
            {
                string downloadedUri = "file:///storage/emulated/0/Download/";
                Java.IO.File file = new Java.IO.File(new Java.Net.URI(downloadedUri + "/" + filename));
                Intent videoPlayerActivity = new Intent(Intent.ActionView);
                videoPlayerActivity.SetDataAndType(Android.Net.Uri.FromFile(file), "video/*");
                Activity activity = Forms.Context as Activity;
                activity.StartActivity(videoPlayerActivity);
                return true;
            }
          
            App.DownloadID = 0;

       

            Android.Net.Uri contentUri = Android.Net.Uri.Parse(uri);

            Android.App.DownloadManager.Request r = new Android.App.DownloadManager.Request(contentUri);


            r.SetDestinationInExternalPublicDir(Android.OS.Environment.DirectoryDownloads, filename);

            r.AllowScanningByMediaScanner();

            r.SetNotificationVisibility(Android.App.DownloadVisibility.VisibleNotifyCompleted);

            Android.App.DownloadManager dm = (Android.App.DownloadManager)Xamarin.Forms.Forms.Context.GetSystemService(Android.Content.Context.DownloadService);

            App.DownloadID = dm.Enqueue(r);

            AndHUD.Shared.Show(MainActivity.GetMainActivity(), "Dowloading video...", -1, MaskType.Clear, null, () => { dm.Remove(App.DownloadID); }, true, () => { dm.Remove(App.DownloadID); });


            return true;
        }

        void OnProgresClick()
        {
            string test = "click";
        }

        void OnProgresCancel()
        {
            string test = "cancel";
        }

        public void PlayVideo(string path)
        {

        }
    }
}