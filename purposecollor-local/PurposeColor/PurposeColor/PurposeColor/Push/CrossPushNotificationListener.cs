using PushNotification.Plugin;
using PushNotification.Plugin.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotifictionListener
{
    public class CrossPushNotificationListener : IPushNotificationListener
    {
        public void OnMessage(IDictionary Parameters, DeviceType deviceType)
        {
            string messge = "new messege";
        }

        public  void OnMessage(IDictionary<string, object> Parameters, DeviceType deviceType)
        {
            string messge = "new messege";
        }

        public async void OnRegistered(string Token, DeviceType deviceType)
        {
            string messge = "new messege";
        }
        public async void OnUnregistered(DeviceType deviceType)
        {
            string messge = "new messege";
        }
        public void OnError(string message, DeviceType deviceType)
        {
            string messge = "new messege";
        }

        public bool ShouldShowNotification()
        {
            return true;
        }
    }
}
