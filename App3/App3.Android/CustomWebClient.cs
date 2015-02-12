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
using Android.Webkit;

namespace App3.Droid
{
    public class CustomWebClient : WebViewClient
    {
        private AltinnActivity mainActivity;

        public CustomWebClient(AltinnActivity mainActivity)
        {
            // TODO: Complete member initialization
            this.mainActivity = mainActivity;
        }


        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {


            if (new Uri(url).Host != null
                        && new Uri(url).Host.Equals("idporten.difi.no")
                        || new Uri(url).Host.Equals("www.altinn.no"))
            {
                return false;
            }

            else
            {
                var uri = Android.Net.Uri.Parse(url);
                var intent = new Intent(Intent.ActionView, uri);
                mainActivity.StartActivity(intent);
                return true;
            }
        }





      //  public override void OnReceivedSslError(WebView view, SslErrorHandler handler, Android.Net.Http.SslError error)
   //     {
     //       handler.Proceed();
     //   }

        public override void OnPageFinished(WebView view, string url)
        {
            CookieSyncManager.Instance.Sync();



            // Henter cookier og sjekker om de validerer
            String cookie = CookieManager.Instance.GetCookie(url);
            //Console.WriteLine(cookie + "Webclient cookies");
            if (((cookie.Contains(".ASPXAUTH") == true && cookie
                            .Contains("altinnContext") == true)))
            {
                //					url.equals(apiUrl)||
                Intent loginIntent = new Intent(mainActivity, typeof(MessageView));
                mainActivity.StartActivity(loginIntent);
                mainActivity.Finish();
            }
        }
    }
}