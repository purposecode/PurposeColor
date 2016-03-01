
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PurposeColor.interfaces;
using PurposeColor.iOS;
using UIKit;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(IOSLocalNotificationsImpl))]
namespace PurposeColor.iOS
{
	class IOSLocalNotificationsImpl : ILocalNotification
	{
		public void ShowNotification(string title, string messege, bool handleClickNeeded )
		{
			try
			{
				UILocalNotification notification = new UILocalNotification();
				notification.FireDate = NSDate.Now;
				notification.AlertBody = messege;
				notification.SoundName = UILocalNotification.DefaultSoundName;
				UIApplication.SharedApplication.ScheduleLocalNotification(notification);
			} 
			catch (Exception ex)
			{
				string err = ex.Message;
			}

		}

	}
}