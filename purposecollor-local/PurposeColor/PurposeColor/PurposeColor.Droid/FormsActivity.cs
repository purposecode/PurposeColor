
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
using PurposeColor;

namespace Native2Forms
{
	/// <summary>
	/// This is a Xamarin.Forms screen. It MUST:
	/// * inherit from ANdroidActivity
	/// * call Forms.Init()
	/// * use SetPage()
	/// </summary>
	[Activity (Label = "FormsActivity")]			
	public class FormsActivity : Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			string message = Intent.GetStringExtra ("Message") ?? "Data not available";
			string fromID = Intent.GetStringExtra ("FromID") ?? "Data not available";


			Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new AppTest ( message, fromID ));
		}
	}
}

