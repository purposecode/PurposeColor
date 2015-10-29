using Android.Content.Res;
using Android.Telephony;
using Cross.Store;
using PurposeColor.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
[assembly: Dependency(typeof(DeviceCommunications))]
namespace Cross.Store
{
    class DeviceCommunications : IDeviceCommunication
    {
        public void SendSMS(string messege, string sender)
        {
            SmsManager.Default.SendTextMessage( sender, null,messege , null, null);
        }

        public void ReadSMS(string searchString)
        {
                
        }
    }
}
