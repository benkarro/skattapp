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
using Android.Telephony;

namespace App3.Droid
{
    [BroadcastReceiver]
    [IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED" })]
    class SMSreader: BroadcastReceiver
    {
        private const string SMS_RECEIVED = "android.provider.Telephony.SMS_RECEIVED";
        private String pass;

        public String getPass()
        {
            return pass;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            Bundle bundle =  intent.Extras;

            if (bundle != null) {
				// get sms objects
                Java.Lang.Object[] pdus = (Java.Lang.Object[])bundle.Get("pdus");
				if (pdus.Length == 0) {
					return;
				}
				// large message might be broken into many
				SmsMessage[] messages = new SmsMessage[pdus.Length];
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < pdus.Length; i++) {
                   messages[i] = SmsMessage.CreateFromPdu((byte[])pdus[i]);

					sb.Append(messages[i].DisplayMessageBody);
				}
				String sender = messages[0].DisplayOriginatingAddress;
				String message = sb.ToString();
				// apply sms filter
				if (PhoneNumberUtils.Compare("26999", sender)  || 
				    PhoneNumberUtils.Compare ("26998", sender)
					&& AltinnActivity.active) {

					pass = message.Substring(0, 5);
					// System.out.println(pass);
					Intent newintent = new Intent("SmsMessage.intent.MAIN").PutExtra("pass", pass);
					context.SendBroadcast(newintent);
					
				

				}
			}


        }

    }
}