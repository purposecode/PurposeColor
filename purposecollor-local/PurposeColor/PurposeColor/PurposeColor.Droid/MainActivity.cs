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


namespace PurposeColor.Droid
{
    [Activity(Label = "PurposeColor", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        static Activity curentActivity;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
           
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.Forms.Forms.SetTitleBarVisibility(AndroidTitleBarVisibility.Never);
            curentActivity = this;
            CrossPushNotification.Initialize<CrossPushNotificationListener>("469628380816");
            ImageCircleRenderer.Init();
            LoadApplication(new App());
        }

        public static Activity GetMainActivity()
        {
            return curentActivity;
        }
    }
}

