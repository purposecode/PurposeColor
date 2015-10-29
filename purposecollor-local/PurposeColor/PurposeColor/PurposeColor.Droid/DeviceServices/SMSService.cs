using Android.Content;
using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Telephony;
using Android.Provider;
using Java.Lang;

[BroadcastReceiver(Enabled = true, Label = "SMS Receiver")]
[IntentFilter(new[] { "android.provider.Telephony.SMS_RECEIVED" })]
public class SMSBroadcastReceiver : BroadcastReceiver
{

    private const string Tag = "SMSBroadcastReceiver";
    private const string IntentAction = "android.provider.Telephony.SMS_RECEIVED";

    public override void OnReceive(Context context, Intent intent)
    {

        if (intent.Action != IntentAction) return;

        SmsMessage[] messages = Telephony.Sms.Intents.GetMessagesFromIntent(intent);

        var sb = new StringBuilder();

        for (var i = 0; i < messages.Length; i++)
        {

        }

    }
}