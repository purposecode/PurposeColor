
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
using Android.Provider;
using PurposeColor.Droid;
using PurposeColor.screens;


[assembly: Xamarin.Forms.Dependency(typeof(AndroidContactPicker))]
namespace PurposeColor.Droid
{


	public class AndroidContactPicker : IContactPicker
	{
		public  void ShowContactPicker()
		{
			Intent intent = new Intent ( Application.Context , typeof(  ContactPicker ));

			MainActivity.GetMainActivity ().StartActivity ( intent );
		}
	}


	[Activity (Label = "ContactPicker")]			
	public class ContactPicker : Activity
	{
		const int CONTACT_PICKER_CODE = 100;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Intent contactPickerIntent = new Intent(Intent.ActionPick,ContactsContract.CommonDataKinds.Phone.ContentUri);
			this.StartActivityForResult ( contactPickerIntent, CONTACT_PICKER_CODE );
			// Create your application here
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (resultCode == Result.Ok) 
			{
				switch( requestCode )
				{
				case CONTACT_PICKER_CODE: 
					OnContactsSelected ( data );
					break;
				}
			}
			else if( resultCode == Result.Canceled )
			{
				this.Finish ();			
			}

		}


		void OnContactsSelected( Intent contactIntent )
		{
			Android.Net.Uri uri = contactIntent.Data;
			Android.Database.ICursor cursor =  ContentResolver.Query ( uri, null, null, null, null );
			cursor.MoveToFirst ();
			//int nameIndex = cursor.GetColumnIndex ( ContactsContract.CommonDataKinds.Phone.Number );
			List<string> columsList =  cursor.GetColumnNames ().ToList();
			int nameIndex = cursor.GetColumnIndex ("display_name");
			string name = cursor.GetString ( nameIndex );
			string test = "test";
			this.Finish ();
			AddEventsSituationsOrThoughts.contactSelectAction ( name );
		}
	}
}

