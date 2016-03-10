﻿using System;
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
			if (!dir.Exists ())
				dir.Mkdirs ();
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

