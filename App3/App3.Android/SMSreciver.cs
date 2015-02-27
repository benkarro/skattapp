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

namespace App3.Droid
{
    class SMSreciver : BroadcastReceiver
    {
        private Activity mainActivity;
       
        private Android.Webkit.WebView webView;

        public SMSreciver(Activity mainActivity, Android.Webkit.WebView webView)
        {
            // TODO: Complete member initialization
            this.mainActivity = mainActivity;
            this.webView = webView;
        }

      
        public override void OnReceive(Context context, Android.Content.Intent intent)
        {
///OLD
				/*webView.LoadUrl("javascript:document.getElementById(\"input_ONETIMECODE_IDPORTEN\").value = '"
								+ pass + "';");*/

                Toast toast = Toast.MakeText(context, "Kode lagt inn i feltet",
                            ToastLength.Long);
                toast.Show();
                String pass = intent.GetStringExtra("pass");
                webView.EvaluateJavascript("javascript:document.getElementById(\"input_ONETIMECODE_IDPORTEN\").value = '" + pass + "';", null);

           
        }

    }
}