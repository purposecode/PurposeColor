
using Cross.Store;
using Microsoft.Phone.Tasks;
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
            SmsComposeTask smsComposeTask = new SmsComposeTask();
            smsComposeTask.To = sender;
            smsComposeTask.Body = messege;
            smsComposeTask.Show();
        }
    }
}
