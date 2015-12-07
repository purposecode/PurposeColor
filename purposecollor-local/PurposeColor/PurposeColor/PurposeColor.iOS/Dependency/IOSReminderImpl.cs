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
		protected static EKEventStore eventStore;

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
			EKEvent newEvent = EKEvent.FromStore(this.EventStore);
			newEvent.StartDate = (NSDate)DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
			newEvent.EndDate = (NSDate)DateTime.SpecifyKind(endtDate, DateTimeKind.Utc);
			newEvent.Title = title;
			newEvent.Notes = message;
			newEvent.AllDay = false;
			//newEvent.AddAlarm(ConvertReminder(reminder, startDate));

			return true;
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
