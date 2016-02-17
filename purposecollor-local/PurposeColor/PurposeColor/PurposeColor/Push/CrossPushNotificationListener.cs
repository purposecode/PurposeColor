using PushNotification.Plugin;
using PushNotification.Plugin.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurposeColor.Model;
using PurposeColor;
using PurposeColor.Service;
using Xamarin.Forms;
using System.Diagnostics;
using PurposeColor.interfaces;

namespace PushNotifictionListener
{
	public class CrossPushNotificationListener : IPushNotificationListener
	{
		public void OnMessage(IDictionary Parameters, DeviceType deviceType)
		{
			string messge = "new messege";

			foreach (KeyValuePair<string, int> pair in Parameters)
			{
				Debug.WriteLine("{0}, {1}", pair.Key, pair.Value);
			}
		}

		public  void OnMessage(IDictionary<string, object> Parameters, DeviceType deviceType)
		{
			string messge = "new messege";

			int index = 0;
			foreach (var pair in Parameters)
			{
				if (index < Parameters.Count - 1) 
				{
					string header = pair.Key.ToString ();
					if (header == "followrequest_id")
						App.NotificationReqID = pair.Value.ToString ();
					else if (header == "message")
						messge = pair.Value.ToString ();

					MessagingCenter.Send<CrossPushNotificationListener, string>(this, "boom", pair.Value.ToString());
					index++;
				}

			}

			ILocalNotification notify = DependencyService.Get<ILocalNotification> ();
			notify.ShowNotification ( "Purpose Color", messge );
		}

		public async void OnRegistered(string Token, DeviceType deviceType)
		{
			User user = App.Settings.GetUser ();
			App.NotificationToken = Token;
			Debug.WriteLine ( "#################################token----------------" + Token );
			if (user != null)
			{
				IProgressBar progres = DependencyService.Get<IProgressBar> ();
				progres.ShowToast ( "token updated" );
				await ServiceHelper.SendNotificationToken (Token);
			}
		}
		public async void OnUnregistered(DeviceType deviceType)
		{
			string messge = "new messege";
		}
		public void OnError(string message, DeviceType deviceType)
		{
			string messge = "new messege";
		}

		public bool ShouldShowNotification()
		{
			return false;
		}
	}
}
