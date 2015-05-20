using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using Android.Webkit;
using Android.Content.PM;

namespace App3.Droid
{
    [Activity(MainLauncher = false, NoHistory = true, 
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize
        /*, ScreenOrientation = ScreenOrientation.Portrait*/)]
    public class AltinnActivity : Activity
    {
        BroadcastReceiver mIntentReciver;
        WebView webView;
		public static Boolean active =false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
			if (((int)Android.OS.Build.VERSION.SdkInt) < 21) {
				SetTheme (Resource.Style.MyThemeNormal); // For deactivation of translucent theme for this activity/layout
			}

            
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Altinn);


            CookieManager.Instance.RemoveExpiredCookie();
			CookieSyncManager.CreateInstance(this);
            CookieSyncManager.Instance.Sync();
            CookieSyncManager.Instance.StartSync();

            webView = FindViewById<WebView>(Resource.Id.webView);
            webView.SetWebViewClient(new CustomWebClient(this));
            




            if (savedInstanceState != null)
            {
                webView.RestoreState(savedInstanceState);
            }
            else
            {
                webView.Settings.JavaScriptEnabled = true;


                // Use subclassed WebViewClient to intercept hybrid native calls





                webView.LoadUrl("https://www.altinn.no/api/my/messages");


                Console.WriteLine("####################################################");
                Console.WriteLine("POKÈMON: " + webView.Url);
                Console.WriteLine("####################################################");


            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            webView.SaveState(outState);
        }

        /*protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            webView.RestoreState(savedInstanceState);
        }*/

        

        
	public override void OnBackPressed() {
		if (webView.CanGoBack())
			webView.GoBack();
		else
			base.OnBackPressed();
	}


    protected override void OnPause(){
		base.OnPause();

		CookieSyncManager.Instance.Sync();
        UnregisterReceiver(mIntentReciver);
			active = false;
        
    }

    protected override void OnResume()
    {
        base.OnResume();
        CookieSyncManager.Instance.StopSync();
			active = true;

        //Ta imot og behandle kode fra SMS
        IntentFilter intentFilter = new IntentFilter("SmsMessage.intent.MAIN");
        mIntentReciver = new SMSreciver(this, webView);
        RegisterReceiver(mIntentReciver, intentFilter);

    }

		protected override void OnStop ()
		{
			base.OnStop ();
			active = false;
		}

		protected override void OnStart ()
		{
			base.OnStart ();
			active = true;
		}
			


    }
		
}




