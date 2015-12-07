using PurposeColor.interfaces;
using PurposeColor.iOS.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using EventKit;
using Foundation;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(IOSReminderImpl))]
namespace PurposeColor.iOS.Dependency
{
    public class IOSReminderImpl : IReminderService
    {
		public EKEventStore EventStore
		{
			get { return eventStore; }
		}
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
			/*EKEvent newEvent = EKEvent.FromStore(this.EventStore);
			newEvent.StartDate = (NSDate)DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
			newEvent.EndDate = (NSDate)DateTime.SpecifyKind(endtDate, DateTimeKind.Utc);
			newEvent.Title = title;
			newEvent.Notes = message;
			newEvent.AllDay = false;
			//newEvent.AddAlarm(EKAlarm.FromDate((NSDate)startDate.AddMinutes(-10)));
			NSError error;
			this.EventStore.SaveEvent(newEvent, EKSpan.ThisEvent, out error);

			return true;*/

			eventController.EventStore = EKEvent.FromStore ( this.EventStore );

			EKEvent newEvent = EKEvent.FromStore ( this.EventStore );
			// set the alarm for 10 minutes from now
			//newEvent.AddAlarm ( EKAlarm.FromDate ( DateTimeToNSDate( startDate.AddMinutes( -15 ) )));
			// make the event start 20 minutes from now and last 30 minutes
			newEvent.StartDate = DateTimeToNSDate( startDate );
			newEvent.EndDate = DateTimeToNSDate( endtDate );
			newEvent.Title = title;
			newEvent.Notes = message;
			newEvent.Calendar = this.EventStore.DefaultCalendarForNewEvents;
			NSError e;
			this.EventStore.SaveEvent ( newEvent, EKSpan.ThisEvent, out e );


			EKReminder ekReminder = EKEvent.FromStore ( this.EventStore );
			ekReminder.Title = title;
			ekReminder.Calendar = this.EventStore.DefaultCalendarForNewEvents;
			this.EventStore.SaveReminder ( ekReminder, true, out e );

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

		/*private EKAlarm ConvertReminder(AppointmentReminder reminder, DateTime startTime)
		{
			switch (reminder)
			{
			case AppointmentReminder.none:
				return EKAlarm.FromDate((NSDate)startTime); ///todo should this be null?
			case AppointmentReminder.five:
				return EKAlarm.FromDate((NSDate)startTime.AddMinutes(-5));
			case AppointmentReminder.fifteen:
				return EKAlarm.FromDate((NSDate)startTime.AddMinutes(-15));
			case AppointmentReminder.thirty:
				return EKAlarm.FromDate((NSDate)startTime.AddMinutes(-30));
			}
			return EKAlarm.FromDate((NSDate)startTime);
		}*/



    }
}
