using Microsoft.Phone.Scheduler;
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
        public void Remind(DateTime dateTime, DateTime dateTime2, string title, string message)
        {

            string param1Value = title;
            string param2Value = message;
            string queryString = "";
            if (param1Value != "" && param2Value != "")
            {
                queryString = "?param1=" + param1Value + "¶m2=" + param2Value;
            }
            else if (param1Value != "" || param2Value != "")
            {
                queryString = (param1Value != null) ? "?param1=" + param1Value : "?param2=" + param2Value;
            }
            Microsoft.Phone.Scheduler.Reminder reminder = new Microsoft.Phone.Scheduler.Reminder("ServiceReminder");
            reminder.Title = title;
            reminder.Content = message;
            reminder.BeginTime = dateTime;
            reminder.ExpirationTime = dateTime.AddDays(1);
            reminder.RecurrenceType = RecurrenceInterval.None;
            reminder.NavigationUri = new Uri("/MainPage.xaml" + queryString, UriKind.Relative);
            ;

            // Register the reminder with the system.
            ScheduledActionService.Add(reminder);

        }
    }
}
