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
using PurposeColor.WinPhone.Dependency;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidShareVia))]
namespace PurposeColor.Droid.Dependency
{
    public class AndroidShareVia : IShareVia
    {
        ProgressBarImpl progress = new ProgressBarImpl();
        public void ShareMedia(string text, string path, PurposeColor.Constants.MediaType type)
        {
            if (path == null)
                return;

            progress.ShowProgressbar( "Preparing share view.." );
            string filename = System.IO.Path.GetFileName( path );

            Android.Net.Uri uri = Android.Net.Uri.Parse(path);

            string downloadedFolder = "/storage/emulated/0/Download/";
            if( App.DownloadsPath != null && !string.IsNullOrEmpty( App.DownloadsPath ) )
            {
                downloadedFolder = App.DownloadsPath + "/";
            }
         /*   if (System.IO.File.Exists(downloadedFolder + filename))
            {
				ShowChooser(type, downloadedFolder + filename, text, path);
            }
            else
            {
                Android.App.DownloadManager.Request r = new Android.App.DownloadManager.Request(uri);

                r.SetDestinationInExternalPublicDir(Android.OS.Environment.ExternalStorageDirectory.ToString(), filename);

                r.AllowScanningByMediaScanner();

                r.SetNotificationVisibility(Android.App.DownloadVisibility.VisibleNotifyOnlyCompletion);

                Android.App.DownloadManager dm = (Android.App.DownloadManager)Xamarin.Forms.Forms.Context.GetSystemService(Android.Content.Context.DownloadService);

                App.ShareDownloadID = dm.Enqueue(r);

            }*/

			ShowChooser(type, downloadedFolder + filename, text, path);




          /*   MessagingCenter.Subscribe<MyTestReceiver, string>(this, "boom", (page, downpath) =>
             {
                 ShowChooser(type, downpath, text);
             });*/



        }

		void ShowChooser(PurposeColor.Constants.MediaType type, string filename, string text, string Uri )
        {

            progress.HideProgressbar();
            Intent shareIntent = new Intent();
            shareIntent.SetAction(Intent.ActionSend);


            // shareIntent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("android.resource://sharetowhtsapp.sharetowhtsapp/" + Resource.Drawable.Icon));

            // file:///storage/emulated/0/Download/ + filename
			string textToShare = "";

			text = text + System.Environment.NewLine + System.Environment.NewLine;
           
            if (type == Constants.MediaType.Video)
            {
                shareIntent.SetType("video/*");
				text = System.Environment.NewLine + text + "   " + Uri + System.Environment.NewLine;
            }
            else if (type == Constants.MediaType.Image)
            {
                shareIntent.SetType("image/*");
				shareIntent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse( "file://" + filename));
				//text = System.Environment.NewLine + text + "   " + Uri + System.Environment.NewLine;
            }
            else if (type == Constants.MediaType.Audio)
            {
                shareIntent.SetType("audio/*");
				text = System.Environment.NewLine + text + "   " + Uri + System.Environment.NewLine;
            }

		     textToShare = text + System.Environment.NewLine +  "shared with purpose color." + System.Environment.NewLine +  " http:\\www.purposecodes.com";
			shareIntent.PutExtra(Intent.ExtraText, textToShare);
            MessagingCenter.Unsubscribe<MyTestReceiver, string>(this, "boom");
            MainActivity.GetMainActivity().StartActivity(Intent.CreateChooser(shareIntent, "Share image via:"));
        }
    }
}