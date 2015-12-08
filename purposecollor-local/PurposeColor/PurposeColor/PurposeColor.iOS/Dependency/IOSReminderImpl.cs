using PurposeColor.interfaces;
using PurposeColor.iOS.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using EventKit;
using Foundation;
using System.Threading.Tasks;
using EventKitUI;
using Xamarin.Forms;
using System.Linq;

[assembly: Xamarin.Forms.Dependency(typeof(IOSReminderImpl))]
namespace PurposeColor.iOS.Dependency
{
    public class IOSReminderImpl : IReminderService
    {
		public EKEventStore EventStore
		{
			get { return eventStore; }
		}

		CreateEventEditViewDelegate eventControllerDelegate;
		protected static EKEventStore eventStore = new EKEventStore();

		EventKitUI.EKEventEditViewController eventController = 	new EventKitUI.EKEventEditViewController ();


		public Task<bool> RequestAccessAsync()
		{
			var taskSource = new TaskCompletionSource<bool>();

			this.EventStore.RequestAccess(EKEntityType.Event,
				(bool granted, NSError e) =>
				{
					if (!granted)
						taskSource.SetException(new Exception("User Denied Access to Calendar Data"));
					else
						taskSource.SetResult(true);
				});

			return taskSource.Task;
		}

		public bool Remind(DateTime startDate, DateTime endtDate, string title, string message, int reminder)
        {

			 Device.BeginInvokeOnMainThread(() =>
			{
				TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
				try
				{
						EKEvent newEvent = EKEvent.FromStore ( this.EventStore );
						// make the event start 20 minutes from now and last 30 minutes
						newEvent.StartDate = DateTimeToNSDate( startDate.ToUniversalTime() );
						newEvent.EndDate = DateTimeToNSDate( endtDate.ToUniversalTime() );
						newEvent.Title = title;
						newEvent.Notes = message;
						newEvent.Calendar = this.EventStore.DefaultCalendarForNewEvents;

						EventKitUI.EKEventEditViewController eventController = new EventKitUI.EKEventEditViewController();
						eventController.EventStore = eventStore;
						eventControllerDelegate = new CreateEventEditViewDelegate (eventController);
						eventController.EditViewDelegate = eventControllerDelegate;
						eventController.Event = newEvent;

						var firstController = UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers.First().ChildViewControllers.Last().ChildViewControllers.First();
		
						firstController.PresentViewController (eventController, true, null);
				}
				catch (Exception ex)
				{
					tcs.SetException(ex);
				}
			});

			return true;


        }


		public static NSDate DateTimeToNSDate( DateTime date)
		{
			if (date.Kind == DateTimeKind.Unspecified)
			{
				date = DateTime.SpecifyKind (date, DateTimeKind.Utc);
			}
			return (NSDate) date;
		}



    }


	// our delegate for the create new event controller.
	public class CreateEventEditViewDelegate : EventKitUI.EKEventEditViewDelegate
	{
		// we need to keep a reference to the controller so we can dismiss it
		protected EventKitUI.EKEventEditViewController eventController;

		public CreateEventEditViewDelegate (EventKitUI.EKEventEditViewController eventController)
		{
			// save our controller reference
			this.eventController = eventController;
		}

		// completed is called when a user eith
		public override void Completed (EventKitUI.EKEventEditViewController controller, EventKitUI.EKEventEditViewAction action)
		{
			eventController.DismissViewController (true, null);

			// action tells you what the user did in the dialog, so you can optionally
			// do things based on what their action was. additionally, you can get the
			// Event from the controller.Event property, so for instance, you could
			// modify the event and then resave if you'd like.
			switch (action) {

			case EventKitUI.EKEventEditViewAction.Canceled:
				break;
			case EventKitUI.EKEventEditViewAction.Deleted:
				break;
			case EventKitUI.EKEventEditViewAction.Saved:
				App.CalPage.Navigation.PopAsync ();
				break;
			}
		}
	}
}
