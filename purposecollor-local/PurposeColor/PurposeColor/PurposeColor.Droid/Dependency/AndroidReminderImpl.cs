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
        public void Remind(DateTime dateTime, string title, string message)
        {
            ContentValues eventValues = new ContentValues();
            eventValues.Put(CalendarContract.Events.InterfaceConsts.CalendarId, 1);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Title, " No ID purpose color event");
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Description, "No ID IReminderService trgered");
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtstart, GetDateTimeMS(2015, 12, 23, 10, 0));
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtend, GetDateTimeMS(2015, 12, 23, 11, 0));
            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventTimezone, "UTC");
            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventEndTimezone, "UTC");
            var uri = Forms.Context.ContentResolver.Insert(CalendarContract.Events.ContentUri, eventValues);

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