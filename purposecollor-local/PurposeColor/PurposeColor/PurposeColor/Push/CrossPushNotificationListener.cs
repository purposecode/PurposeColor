﻿using PushNotification.Plugin;
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
			string messge = null;
			string fromID = null;
			string chat = "";

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
					else if (header == "offline")
						messge = pair.Value.ToString ();
					else if (header == "from_id")
						fromID = pair.Value.ToString ();
					else if (header == "chat") 
					{
						chat = pair.Value.ToString ();
						MessagingCenter.Send<CrossPushNotificationListener, string>(this, "boom", pair.Value.ToString() + "&&" + fromID);
					}
						

					index++;
				}

			}

			if (messge != null) 
			{
				ILocalNotification notify = DependencyService.Get<ILocalNotification> ();
				notify.ShowNotification ("Purpose Color", messge, true);
			} 
			else 
			{
				ILocalNotification sample = DependencyService.Get<ILocalNotification> ();
				sample.ShowNotification ( "Purpose Color", chat, true );
			}

		}

		public async void OnRegistered(string Token, DeviceType deviceType)
		{
			IProgressBar progres = DependencyService.Get<IProgressBar> ();
			progres.ShowProgressbarWithCancel ( "token-->" + Token, () =>{ progres.HideProgressbarWithCancel(); } );


			User user = App.Settings.GetUser ();
			App.NotificationToken = Token;
			if (user != null)
			{
				
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
