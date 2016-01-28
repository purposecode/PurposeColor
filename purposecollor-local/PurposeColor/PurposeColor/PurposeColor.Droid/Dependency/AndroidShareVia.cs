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
using PurposeColor.Droid.Dependency;
using PurposeColor.interfaces;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidShareVia))]
namespace PurposeColor.Droid.Dependency
{
    public class AndroidShareVia : IShareVia
    {
        public void ShareMedia(string text, string path, PurposeColor.Constants.MediaType type)
        {

            string filename = System.IO.Path.GetFileName( path );

            Android.Net.Uri uri = Android.Net.Uri.Parse(path);

            string downloadedFolder = "/storage/emulated/0/Download/";
            if( App.DownloadsPath != null && !string.IsNullOrEmpty( App.DownloadsPath ) )
            {
                downloadedFolder = App.DownloadsPath + "/";
            }
            if (System.IO.File.Exists(downloadedFolder + filename))
            {
                ShowChooser(type, downloadedFolder + filename, text);
            }
            else
            {
                Android.App.DownloadManager.Request r = new Android.App.DownloadManager.Request(uri);

                r.SetDestinationInExternalPublicDir(Android.OS.Environment.ExternalStorageDirectory.ToString(), filename);

                r.AllowScanningByMediaScanner();

                r.SetNotificationVisibility(Android.App.DownloadVisibility.VisibleNotifyCompleted);

                Android.App.DownloadManager dm = (Android.App.DownloadManager)Xamarin.Forms.Forms.Context.GetSystemService(Android.Content.Context.DownloadService);

                App.ShareDownloadID = dm.Enqueue(r);

            }




             MessagingCenter.Subscribe<MyTestReceiver, string>(this, "boom", (page, downpath) =>
             {
                 ShowChooser(type, downpath, text);
             });



        }

        void ShowChooser(PurposeColor.Constants.MediaType type, string filename, string text)
        {
            Intent shareIntent = new Intent();
            shareIntent.SetAction(Intent.ActionSend);
            shareIntent.PutExtra(Intent.ExtraText, text);

            // shareIntent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("android.resource://sharetowhtsapp.sharetowhtsapp/" + Resource.Drawable.Icon));

            // file:///storage/emulated/0/Download/ + filename

            shareIntent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse( "file://" + filename));
            if (type == Constants.MediaType.Video)
            {
                shareIntent.SetType("video/*");
            }
            else if (type == Constants.MediaType.Image)
            {
                shareIntent.SetType("image/*");
            }
            else if (type == Constants.MediaType.Audio)
            {
                shareIntent.SetType("audio/*");
            }

            MessagingCenter.Unsubscribe<MyTestReceiver, string>(this, "boom");
            MainActivity.GetMainActivity().StartActivity(Intent.CreateChooser(shareIntent, "Share image via:"));
        }
    }
}