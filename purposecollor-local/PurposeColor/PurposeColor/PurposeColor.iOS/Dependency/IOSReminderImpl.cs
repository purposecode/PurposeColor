using PurposeColor.interfaces;
using PurposeColor.iOS.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(IOSReminderImpl))]
namespace PurposeColor.iOS.Dependency
{
    public class IOSReminderImpl : IReminderService
    {
		public bool Remind(DateTime startDate, DateTime endtDate, string title, string message, int reminder)
        {
            var notification = new UILocalNotification();

            //---- set the fire date (the date time in which it will fire)
            //notification.FireDate = dateTime;

            //---- configure the alert stuff
            notification.AlertAction = title;
            notification.AlertBody = message;

            //---- modify the badge
            notification.ApplicationIconBadgeNumber = 1;

            //---- set the sound to be the default sound
            notification.SoundName = UILocalNotification.DefaultSoundName;

            //---- schedule it
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
			return true;
        }
    }
}
