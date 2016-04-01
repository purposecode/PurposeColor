using System;
using UIKit;
using AddressBookUI;
using System.Linq;
using PurposeColor.iOS;
using Xamarin.Forms;
using PurposeColor.screens;

[assembly: Xamarin.Forms.Dependency(typeof(IOSContactPicker))]
namespace PurposeColor.iOS
{
	public class IOSContactPicker : IContactPicker
	{
		public  void ShowContactPicker()
		{
			Device.BeginInvokeOnMainThread (() => 
			{
					var firstController = UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers.First().ChildViewControllers.Last().ChildViewControllers.First();

					ABPeoplePickerNavigationController _contactController = new ABPeoplePickerNavigationController ();
					_contactController.Cancelled += (object sender, EventArgs e) => 
					{
						firstController.DismissViewController( true, null );	
					};

					_contactController.SelectPerson2 += (object sender, ABPeoplePickerSelectPerson2EventArgs e) => 
					{
						AddEventsSituationsOrThoughts.contactSelectAction ( e.Person.FirstName );
					};
					firstController.PresentViewController (_contactController, true, null);		
					//_contactController.ShowController();
			});
		}
	}


	public class ChooseCotactViewController : UIViewController
	{
		ABPeoplePickerNavigationController _contactController;
		public ChooseCotactViewController () : base ("ChooseCotactViewController", null)
		{
			_contactController = new ABPeoplePickerNavigationController ();
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}


		public void ShowController()
		{
			var firstController = UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers.First().ChildViewControllers.Last().ChildViewControllers.First();
			firstController.PresentModalViewController (_contactController, true);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

 			



			_contactController.Cancelled += delegate {
				this.DismissModalViewController (true); };



			_contactController.SelectPerson2 += delegate(object sender, ABPeoplePickerSelectPerson2EventArgs e) 
			{

				string name = e.Person.FirstName;
				this.DismissModalViewController (true);
			};
		}

	}
}

