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
using System.Collections.ObjectModel;

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
			User user = App.Settings.GetUser ();
			if (user == null)
				return;
			
			string followMessege = null;
			string fromID = null;
			string chat = null;
			string fromuser = null;
			string offlineMessage = null;


			if (Device.OS == TargetPlatform.iOS)
			{
				// See whether Dictionary contains this string.
				if (Parameters.ContainsKey ("aps")) {
					IDictionary dictionry = (IDictionary)Parameters ["aps"];
					ICollection valuesCollection = dictionry.Values;
					ICollection keysCollection = dictionry.Keys;

					string[] arraykeys = new string[10];

					IList keylist = keysCollection as IList;
					IList valueList = valuesCollection as IList;

					Dictionary<string, string> createdDictionary = new Dictionary<string, string> ();

					for (int itemIndex = 0; itemIndex < valuesCollection.Count; itemIndex++) {
						createdDictionary.Add (keylist [itemIndex].ToString (), valueList [itemIndex].ToString ());
					}

					foreach (var pair in createdDictionary) {
						string val = pair.Value.ToString ();
						Debug.WriteLine (val);
						string header = pair.Key.ToString ();
						if (header == "followrequest_id")
							App.NotificationReqID = pair.Value.ToString ();
						else if (header == "follow")
							followMessege = pair.Value.ToString ();
						else if (header == "offline")
							offlineMessage = pair.Value.ToString ();
						else if (header == "from_id")
							fromID = pair.Value.ToString ();
						else if (header == "from_user")
							fromuser = pair.Value.ToString ();
						else if (header == "chat") {
							chat = pair.Value.ToString ();

						}
					}

				}
			}
			else if (Device.OS == TargetPlatform.Android) 
			{

				foreach (var pair in Parameters)
				{
					string val = pair.Value.ToString ();
					Debug.WriteLine ( val );
					string header = pair.Key.ToString ();
					if (header == "followrequest_id")
						App.NotificationReqID = pair.Value.ToString ();
					else if (header == "follow")
						followMessege = pair.Value.ToString ();
					else if (header == "offline")
						offlineMessage = pair.Value.ToString ();
					else if (header == "from_id")
						fromID = pair.Value.ToString ();
					else if (header == "from_user")
						fromuser = pair.Value.ToString ();
					else if (header == "chat") 
					{
						chat = pair.Value.ToString ();

					}
				}
			}


			if (followMessege != null) 
			{
				// if "accepted" is in the string then that means it is an acknowledgement. then should not proceed
				if (followMessege.Contains ("accepted")) 
				{
					ILocalNotification notify = DependencyService.Get<ILocalNotification> ();
					notify.ShowNotification ("followed", "", followMessege, true);	
				}
				else 
				{
					if (fromID == user.ID.ToString ())
						return;
					
					ILocalNotification notify = DependencyService.Get<ILocalNotification> ();
					notify.ShowNotification ("follow", "", followMessege, true);
				}

			} 
			else if (chat != null) 
			{
				chat = chat + "&&" + fromID;
				string fromUserMessage = fromuser + "  send a chat message";
				MessagingCenter.Send<CrossPushNotificationListener, string>(this, "boom", chat);
				if (App.CurrentChatUserID != null && App.CurrentChatUserID != fromID) 
				{
					ILocalNotification notify = DependencyService.Get<ILocalNotification> ();
					notify.ShowNotification ("chat", fromUserMessage, chat, true);	
				}
				else if( App.CurrentChatUserID == null )
				{
					ILocalNotification notify = DependencyService.Get<ILocalNotification> ();
					notify.ShowNotification ("chat", fromUserMessage, chat, true);	
				}

			} 
			else if( offlineMessage != null )
			{
				ILocalNotification notify = DependencyService.Get<ILocalNotification> ();
				notify.ShowNotification ("offline", offlineMessage, chat, true);	
			}
			else 
			{
				ILocalNotification sample = DependencyService.Get<ILocalNotification> ();
				sample.ShowNotification ( "Purpose Color", "", chat, false );
			}

		}

		public async void OnRegistered(string Token, DeviceType deviceType)
		{

			/*	IProgressBar progres = DependencyService.Get<IProgressBar> ();
			progres.ShowProgressbarWithCancel ( "token-->" + Token, () =>{ progres.HideProgressbarWithCancel(); } );*/


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
