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

[assembly: Xamarin.Forms.Dependency(typeof(AndroidVideoPlayer))]
namespace PurposeColor.Droid.Dependency
{
    class AndroidVideoPlayer : IVideoDownloader
    {
        public async Task<bool> Download(string uri, string filename)
        {

            App.DownloadID = 0;

            AndHUD.Shared.Show(MainActivity.GetMainActivity(), "Dowloading video...", -1, MaskType.Clear  );

            Android.Net.Uri contentUri = Android.Net.Uri.Parse(uri);

            Android.App.DownloadManager.Request r = new Android.App.DownloadManager.Request(contentUri);


            r.SetDestinationInExternalPublicDir(Android.OS.Environment.DirectoryDownloads, filename);

            r.AllowScanningByMediaScanner();

            r.SetNotificationVisibility(Android.App.DownloadVisibility.VisibleNotifyCompleted);

            Android.App.DownloadManager dm = (Android.App.DownloadManager)Xamarin.Forms.Forms.Context.GetSystemService(Android.Content.Context.DownloadService);

            App.DownloadID = dm.Enqueue(r);


            return true;
        }

        public void PlayVideo(string path)
        {

        }
    }
}