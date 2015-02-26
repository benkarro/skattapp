
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

using App3.Parser;
using System.Threading.Tasks;




using Environment = Android.OS.Environment;

 

namespace App3.Droid
{
	[Activity ()]			
	public class SingleMessageView : Activity
	{

		private WebView myWebView;

		JSONparser JsonParser;
        JSONparserSingle JsonParserSingle;
		HttpHelper2 HttpHelper2;
		Task<String> Json;
		String JsonString;
		String messagelink;
        String subject;
        String body;
        string aLabel;
        

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView(Resource.Layout.Altinn);

			myWebView = FindViewById<WebView>(Resource.Id.webView);

		    myWebView.Settings.JavaScriptEnabled = true;

			WebSettings settings = myWebView.Settings;
			settings.DefaultTextEncodingName="utf-8";

			myWebView.SetWebViewClient (new SingleWebClient ());

			myWebView.SetDownloadListener(new SingleDownloadListner(this));

			messagelink = Intent.GetStringExtra ("messagelink");

            aLabel = Intent.GetStringExtra("senderlabel");

            this.Title = aLabel;

			HttpHelper2 = new HttpHelper2(messagelink);

            getJsonStringandParse(HttpHelper2);

            


		}

		public async void getJsonStringandParse(HttpHelper2 http)
		{

			Json=  http.DownloadJson();
			JsonString =await Json;
			Console.WriteLine ("Pokemon " + JsonString);
			JsonParserSingle = new JSONparserSingle(JsonString);

            subject = JsonParserSingle.Subject;
            
            body = JsonParserSingle.Body;

			myWebView.LoadData(subject + "<br/>" + body, "text/html; charset=UTF-8", null);
           



//			Her skal det parses



		}
	}

	public class SingleWebClient : WebViewClient
	{
		public override bool ShouldOverrideUrlLoading (WebView view, string url)
		{
			return false;
		}
	}

				

	public class SingleDownloadListner : Java.Lang.Object, IDownloadListener 
	{
		Context context;
		public SingleDownloadListner(Context context){
			this.context=context;
		}
		public void OnDownloadStart(string url, string userAgent, string contentDisposition, string mimetype, long contentLength)
		{
		
			Environment.GetExternalStoragePublicDirectory (Environment.DirectoryDownloads).Mkdirs ();

			String filename = URLUtil.GuessFileName(url,
				contentDisposition, mimetype).Replace(" ", "");


			String mime= MimeTypeMap.GetFileExtensionFromUrl(filename);

			DownloadManager dm =(DownloadManager)context.GetSystemService(Context.DownloadService);
			DownloadManager.Request request = new DownloadManager.Request (Android.Net.Uri.Parse (url));
			request.AddRequestHeader("Cookie", CookieManager.Instance.GetCookie(url));


			request.SetMimeType (mime);
			Console.WriteLine("MIME: "+ mime);
			request.SetDestinationInExternalPublicDir (Environment.DirectoryDownloads, filename);

			long id= dm.Enqueue(request.SetNotificationVisibility(Android.App.DownloadVisibility.Visible));

			BroadcastReceiver onComplete = new SingleBroadcastReciver (filename, dm, mime,id);

			context.RegisterReceiver(onComplete, new IntentFilter(
				DownloadManager.ActionDownloadComplete));
			
			}



		}

	public class SingleBroadcastReciver: BroadcastReceiver
	{
		public String filename;
		public DownloadManager dm;
		public String mime;
		public long id;
		public SingleBroadcastReciver(String filename, DownloadManager dm, String mime, long id){
			this.filename = filename;
			this.dm = dm;
			this.mime = mime;
			this.id = id;
		}

		public override void OnReceive (Context context, Intent intent)
		{
			Java.IO.File file = new Java.IO.File (Environment.GetExternalStoragePublicDirectory (Environment.DirectoryDownloads).AbsolutePath + "/" + filename);
			Toast.MakeText (context, "Nedlastning Klar", ToastLength.Short).Show ();

			Intent intent1 = new Intent();
			intent1.SetAction (Android.Content.Intent.ActionView);
			intent1.SetDataAndType (dm.GetUriForDownloadedFile (id), mime);
			context.StartActivity (intent1);

		}

	}


}



