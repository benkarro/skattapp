using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using Android.Webkit;

namespace App3.Droid
{
    [Activity(MainLauncher = false)]
    public class AltinnActivity : Activity
    {
        BroadcastReceiver mIntentReciver;
        WebView webView;
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);


            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Altinn);


            CookieManager.Instance.RemoveExpiredCookie();
			CookieSyncManager.CreateInstance(this);
            CookieSyncManager.Instance.Sync();
            CookieSyncManager.Instance.StartSync();

            webView = FindViewById<WebView>(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;
            

            // Use subclassed WebViewClient to intercept hybrid native calls
            webView.SetWebViewClient(new CustomWebClient(this));

            webView.LoadUrl("https://www.altinn.no/api/my/messages");
        }


        

        
	public override void OnBackPressed() {
		if (webView.CanGoBack())
			webView.GoBack();
		else
			base.OnBackPressed();
	}


    protected override void OnPause(){
		base.OnPause();
		MyApplication.activityPaused();

		CookieSyncManager.Instance.Sync();
        UnregisterReceiver(mIntentReciver);
        
    }

    protected override void OnResume()
    {
        base.OnResume();
        MyApplication.activityResumed();
        CookieSyncManager.Instance.StopSync();

        //Ta imot og behandle kode fra SMS
        IntentFilter intentFilter = new IntentFilter("SmsMessage.intent.MAIN");
        mIntentReciver = new SMSreciver(this, webView);

        RegisterReceiver(mIntentReciver, intentFilter);

    }




    }
}




