using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Android_X_Tashtit.BROADCAST
{
    [BroadcastReceiver()]
    [IntentFilter(new[] { TelephonyManager.ActionPhoneStateChanged })]
    public class IncomeCallReciever : BroadcastReceiver
    {
        private static bool messageSent = false;

        public override void OnReceive(Context context, Intent intent)
        {
            //Toast.MakeText(context, "Received intent!", ToastLength.Short).Show();

            // בדיקת אם יש מידע ב-intent
            if (intent.Extras != null)
            {
                // קריאת מצב השיחה
                string state = intent.GetStringExtra(TelephonyManager.ExtraState);

                if (state == TelephonyManager.ExtraStateOffhook)
                    messageSent = true;

                // בדיקת מצב השיחה
                if (state == TelephonyManager.ExtraStateIdle)
                {
                    // קריאת מס' הטלפון המתקשר
                    string telephone = intent.GetStringExtra(TelephonyManager.ExtraIncomingNumber);

                    if (string.IsNullOrEmpty(telephone))
                        telephone = string.Empty;

                    //Toast.MakeText(context, "Calling number: " + telephone, ToastLength.Short).Show();

                    if (!string.IsNullOrEmpty(telephone) && !messageSent)
                    {
                        SmsManager.Default.SendTextMessage(telephone, null, "Sorry, I'm bussy now.\nI'll call you later.", null, null);
                        messageSent = true;
                    }
                }
            }
        }
    }

}