
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
		public void ShowNotification(string title, string messageTitle, string messege, bool handleClickNeeded )
		{
			try
			{
				string chatMsg = messege;
				string chatTouserID = "";
				if( title == "chat" )
				{
					string[] delimiters = { "&&" };
					string[] clasIDArray = messege.Split(delimiters, StringSplitOptions.None);
					chatMsg = clasIDArray [0];
					chatTouserID = clasIDArray [1];
				}
				AppDelegate.CurrentNotificationType = title;
				UILocalNotification notification = new UILocalNotification();
				notification.AlertTitle = messageTitle;
				notification.FireDate = NSDate.Now;
				notification.AlertAction = messageTitle;
				notification.AlertBody = chatMsg;
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