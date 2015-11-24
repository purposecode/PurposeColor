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
using PurposeColor.interfaces;
using PurposeColor.Droid.Dependency;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidLocalNotificationImpl))]
namespace PurposeColor.Droid.Dependency
{
    class AndroidLocalNotificationImpl : ILocalNotification
    {
        public void ShowNotification(string title, string messege)
        {
            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(MainActivity.GetMainActivity())
                                        .SetContentTitle(title)
                                        .SetContentText(messege)
                                        .SetDefaults(NotificationDefaults.Sound)
                                        .SetSmallIcon(Resource.Drawable.app_icon);

            builder.SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate);

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager = (NotificationManager)MainActivity.GetMainActivity().GetSystemService(Context.NotificationService);

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }
       
    }
}