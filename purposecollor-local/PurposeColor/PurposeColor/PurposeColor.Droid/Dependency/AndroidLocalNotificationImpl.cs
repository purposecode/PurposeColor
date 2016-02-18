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
			try
			{
				// Set up an intent so that tapping the notifications returns to this app:
				Intent intent = new Intent ( Application.Context , typeof(  NotificationClick ));
				//intent.RemoveExtra ("MyData");
				intent.PutExtra ("MyData", messege);

				// Create a PendingIntent; we're only using one PendingIntent (ID = 0):
				const int pendingIntentId = 0;
				PendingIntent pendingIntent = 
					PendingIntent.GetActivity ( MainActivity.GetMainActivity() , pendingIntentId, intent, PendingIntentFlags.OneShot);

				// Instantiate the builder and set notification elements:
				Notification.Builder builder = new Notification.Builder(MainActivity.GetMainActivity())
					.SetContentIntent( pendingIntent )
					.SetContentTitle(title)
					.SetContentText(messege)
					.SetDefaults(NotificationDefaults.Sound)
					.SetAutoCancel( true )
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
			catch (Exception ex)
			{
				string err = ex.Message;
			}

		}

	}
}