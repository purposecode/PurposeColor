
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
using Xamarin.Forms;
using PushNotifictionListener;
using PurposeColor.Model;
using Native2Forms;

namespace PurposeColor.Droid
{
	[Activity (Label = "NotificationClick")]			
	public class NotificationClick : Activity
	{
		static Activity curentActivity;
		protected override void OnCreate (Bundle bundle)
		{

			RequestWindowFeature(WindowFeatures.NoTitle);
			//Xamarin.Forms.Forms.SetTitleBarVisibility(AndroidTitleBarVisibility.Never);
			curentActivity = this;
			base.OnCreate (bundle);

			string message = Intent.GetStringExtra ("Message") ?? "Data not available";
			string title = Intent.GetStringExtra ("Title") ?? "Data not available";


			if (title == "chat")
			{
				Intent intent = new Intent ( Android.App.Application.Context , typeof(  FormsActivity ));


				string[] delimiters = { "&&" };
				string[] clasIDArray = message.Split(delimiters, StringSplitOptions.None);
				string selectedMessage = clasIDArray [0];
				string selectedFromID = clasIDArray [1];

				//intent.RemoveExtra ("MyData");
				intent.PutExtra ("FromID", selectedFromID);
				intent.PutExtra ("Message", selectedMessage);

				StartActivity ( intent );
			}
			else if (title == "follow")
			{

				LinearLayout layout = FindViewById<LinearLayout>(Resource.Id.MyLayout);
				if( layout != null )
					layout.SetBackgroundColor ( Android.Graphics.Color.Rgb( 244, 244, 244 ) );

				//set alert for executing the task
				AlertDialog.Builder alert = new AlertDialog.Builder (this);

				alert.SetTitle ( "Purpose Color" );

				alert.SetPositiveButton ("Accept", async  (senderAlert, args) => 
					{
						await App.UpdateNotificationStatus(  App.NotificationReqID , "2") ;
					} );

				alert.SetNegativeButton ("Reject", async (senderAlert, args) => 
					{
						await App.UpdateNotificationStatus( "0", App.NotificationReqID ) ;
					} );
				//run the alert in UI thread to display in the screen
				RunOnUiThread (() => {
					alert.Show();
				} );

				SetContentView ( Resource.Layout.samplenotificationclick );
			}



			// Create your application here
		}

		public static Activity GetNotificationClick()
		{
			return curentActivity;
		} 
	}
}

