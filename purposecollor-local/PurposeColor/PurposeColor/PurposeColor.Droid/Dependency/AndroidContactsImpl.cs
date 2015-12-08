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
using Contacts.Plugin.Abstractions;
using System.Threading.Tasks;
using Xamarin.Contacts;
using PurposeColor.Droid.Dependency;
using PurposeColor.interfaces;


[assembly: Xamarin.Forms.Dependency(typeof(AndroidContactsImpl))]
namespace PurposeColor.Droid.Dependency
{
    class AndroidContactsImpl : IDeviceContacts
    {
        public async Task<List<string>> GetContacts()
        {
            List<string> contactList = null;
            try
            {

                contactList = new List<string>();
                var book = new AddressBook(MainActivity.GetMainActivity());


                if (!await book.RequestPermission())
                {
                    Toast.MakeText(MainActivity.GetMainActivity(), "Permission denied.", ToastLength.Short);
                    return null;
                }

                //foreach (Xamarin.Contacts.Contact contact in book.OrderBy(c => c.LastName))
                try
                {
                    book.OrderBy(c => c.DisplayName);
                }
                catch (Exception ex)
                {
                    var test = ex.Message;
                }

                foreach (Xamarin.Contacts.Contact contact in book)
                {
                    if (contact.FirstName != null && contact.FirstName.Trim().Length > 0)
                        contactList.Add(contact.FirstName);
                }

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
            return contactList;
        }
    }
}