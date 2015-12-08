using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Tasks;
using PurposeColor.interfaces;
using PurposeColor.WinPhone.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[assembly: Xamarin.Forms.Dependency(typeof(WinReminderImpl))]
namespace PurposeColor.WinPhone.Dependency
{
    public class WinReminderImpl : IReminderService
    {
		public Task<bool> RequestAccessAsync ()
		{
            return Task.Delay(10)
          .ContinueWith(t => true);
		}

        public  bool Remind(DateTime startDate, DateTime endtDate, string title, string message, int reminderVal)
        {

            SaveAppointmentTask saveAppointmentTask = new SaveAppointmentTask();

            saveAppointmentTask.StartTime = startDate;
            saveAppointmentTask.EndTime = endtDate;
            saveAppointmentTask.Subject = title;
            saveAppointmentTask.Details = message;
            saveAppointmentTask.IsAllDayEvent = false;
            saveAppointmentTask.Reminder = ConvertReminder(reminderVal);

            saveAppointmentTask.Show();

            return true;
        }

        private Microsoft.Phone.Tasks.Reminder ConvertReminder(int reminder)
        {
            switch (reminder)
            {
                case 0:
                    return Microsoft.Phone.Tasks.Reminder.None;
                case 15:
                    return Microsoft.Phone.Tasks.Reminder.FifteenMinutes;
                case 30 :
                    return Microsoft.Phone.Tasks.Reminder.ThirtyMinutes;
                case 45:
                    return Microsoft.Phone.Tasks.Reminder.ThirtyMinutes;
                case 60:
                    return Microsoft.Phone.Tasks.Reminder.OneHour;
            }
            return Microsoft.Phone.Tasks.Reminder.AtStartTime;
        }
    }
}
