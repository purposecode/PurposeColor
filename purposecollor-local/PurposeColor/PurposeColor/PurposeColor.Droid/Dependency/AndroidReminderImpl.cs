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
using PurposeColor.interfaces;
using PurposeColor.Droid.Renderers;
using Android.Provider;
using Java.Util;
using MonoDroid;
using Xamarin.Forms;


[assembly: Xamarin.Forms.Dependency(typeof(AndroidReminderImpl))]
namespace PurposeColor.Droid.Renderers
{
    public class AndroidReminderImpl : IReminderService
    {
		public Task<bool> RequestAccessAsync ()
		{
			return true;
		}

        public bool Remind(DateTime startDate, DateTime endtDate, string title, string message, int reminder)
        {
            try
            {
                ContentValues eventValues = new ContentValues();
                eventValues.Put(CalendarContract.Events.InterfaceConsts.CalendarId, 1);
                // eventValues.Put(CalendarContract.Events.InterfaceConsts.AllDay, 1);
                eventValues.Put(CalendarContract.Events.InterfaceConsts.HasAlarm, 1);
                eventValues.Put(CalendarContract.Events.InterfaceConsts.Title, title);
                eventValues.Put(CalendarContract.Events.InterfaceConsts.Description, message);
                eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtstart, GetDateTimeMS(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute));
                //  eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtend,  GetDateTimeMS(endtDate.Year, endtDate.Month, endtDate.Day, endtDate.Hour, endtDate.Minute));
                eventValues.Put(CalendarContract.Events.InterfaceConsts.EventTimezone, "UTC");
                eventValues.Put(CalendarContract.Events.InterfaceConsts.EventEndTimezone, "UTC");



                string until = endtDate.ToString("yyyyMMdd");

                eventValues.Put(CalendarContract.Events.InterfaceConsts.Rrule, "FREQ=DAILY;UNTIL=" + until);//+  endtDate.Year.ToString()+ endtDate.Month.ToString() + endtDate.Day.ToString());
                eventValues.Put(CalendarContract.Events.InterfaceConsts.Duration, "+P30M");

                var uri = Forms.Context.ContentResolver.Insert(CalendarContract.Events.ContentUri, eventValues);

                long eventID = long.Parse(uri.LastPathSegment);
                // reminder insert
                Uri REMINDERS_URI = new Uri(CalendarContract.Reminders.ContentUri + "reminders");
                ContentValues remindervalues = new ContentValues();
                // remindervalues.Put(CalendarContract.Reminders.InterfaceConsts.AllDay, 1);
                remindervalues.Put(CalendarContract.Reminders.InterfaceConsts.Minutes, reminder);
                remindervalues.Put(CalendarContract.Reminders.InterfaceConsts.EventId, eventID);
                remindervalues.Put(CalendarContract.Reminders.InterfaceConsts.Method, (int)Android.Provider.RemindersMethod.Alert);
                var reminderURI = Forms.Context.ContentResolver.Insert(CalendarContract.Reminders.ContentUri, remindervalues);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }


        }

        long GetDateTimeMS(int yr, int month, int day, int hr, int min)
        {
            Calendar c = Calendar.GetInstance(Java.Util.TimeZone.Default);

            c.Set(Calendar.DayOfMonth, day);
            c.Set(Calendar.HourOfDay, hr);
            c.Set(Calendar.Minute, min);
            c.Set(Calendar.Month, month - 1);
            c.Set(Calendar.Year, yr);

            return c.TimeInMillis;
        }
    }
}