
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
using Uri = Android.Net.Uri;


using App3.Parser;
using System.Threading.Tasks;




using Environment = Android.OS.Environment;
using Android.Support.V7.App;

 

namespace App3.Droid
{
	[Activity ()]
	public class SingleMessageView : Activity
	{

		private WebView myWebView;

		JSONparserSingle JsonParserSingle;
		HttpHelper2 HttpHelper2;
		Task<String> Json;
		String JsonString;
		String messagelink;
        String subject;
        String body;
		List<Attachment> attachmentList;
		String attachmentString="";


        string aLabel;
        

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView(Resource.Layout.Altinn);

			ActionBar.SetDisplayHomeAsUpEnabled (true);

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
            Console.Write(Json);
			Json=  http.DownloadJson();
			JsonString =await Json;
			//Console.WriteLine ("Pokemon " + JsonString);
			JsonParserSingle = new JSONparserSingle(JsonString);

            subject = JsonParserSingle.Subject;
            
			body = JsonParserSingle.Body;

			attachmentList = JsonParserSingle._links.attachment;
			if (attachmentList != null) {
				foreach (Attachment attachment in attachmentList) {
					

					attachmentString+= "<br/>" + String.Format("<a href=\" {0} \">{1}</a>", attachment.href,attachment.name);

					//String.Format("<a href=\" {0} \"><img src=\"{1}\" width=\"300\" height=\"200\" border=\"0\"/> </a>",attachment.href,"")+


				}
			}

		//	if(attachmentList != null){
		//		attachment

		//	}


		

						myWebView.LoadData(subject + "<br/>" + body + "<br/>"+ attachmentString , "text/html; charset=UTF-8", null);
           



//			Her skal det parses



		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater inflater = MenuInflater;
			inflater.Inflate (Resource.Menu.actionbar_buttons, menu);
			return base.OnCreateOptionsMenu (menu);
		}


		public override bool OnOptionsItemSelected (IMenuItem item)
		{

			switch (item.ItemId)
			{
			case Android.Resource.Id.Home:
				Finish();
				return true;

			case Resource.Id.logout:
				CookieManager.Instance.RemoveSessionCookie();
				StartActivity(typeof(MainActivity));
				Finish();
				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
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
		readonly Context context;
		public SingleDownloadListner(Context context){
			this.context=context;
		}
		public void OnDownloadStart(string url, string userAgent, string contentDisposition, string mimetype, long contentLength)
		{
		
			Environment.GetExternalStoragePublicDirectory (Environment.DirectoryDownloads).Mkdirs ();


			String filename = URLUtil.GuessFileName(url,
				contentDisposition, mimetype).Replace(" ", "");



			String mime= MimeTypeMap.GetFileExtensionFromUrl(filename);

            Console.WriteLine("THIS MIME :" + mime);

			DownloadManager dm =(DownloadManager)context.GetSystemService(Context.DownloadService);
			DownloadManager.Request request = new DownloadManager.Request (Android.Net.Uri.Parse (url));
			request.AddRequestHeader("Cookie", CookieManager.Instance.GetCookie(url));


			request.SetMimeType (mime);
			Console.WriteLine("MIME: "+ mime);

            if (mime == null || mime == "")
            {

                
                AlertDialog.Builder albuilder = new AlertDialog.Builder(context);
                albuilder.SetTitle("Bekreftelse");
                albuilder.SetMessage("Filen mangler filformat, vil du prøve å åpne den som en pdf? det vil kanskje ikke virke..");
                albuilder.SetCancelable(false);
                albuilder.SetPositiveButton("Ja", (object sender, DialogClickEventArgs e) =>
                {


                    mime = "pdf";

                    request.SetDestinationInExternalPublicDir(Environment.DirectoryDownloads, filename + ".pdf");

                    long id = dm.Enqueue(request.SetNotificationVisibility(Android.App.DownloadVisibility.Visible));

                    BroadcastReceiver onComplete = new SingleBroadcastReciver(filename + ".pdf", dm, mime, id);

                    context.RegisterReceiver(onComplete, new IntentFilter(
                        DownloadManager.ActionDownloadComplete));


                });

                albuilder.SetNegativeButton("Nei", (object sender, DialogClickEventArgs e) =>
                {
                    Toast.MakeText(context, "Operasjon kanselert..", ToastLength.Short).Show();
                });


                AlertDialog AlertBox = albuilder.Create();
                AlertBox.Show();
               


            }
            else 
            {

			    request.SetDestinationInExternalPublicDir (Environment.DirectoryDownloads, filename);

			    long id= dm.Enqueue(request.SetNotificationVisibility(Android.App.DownloadVisibility.Visible));

			    BroadcastReceiver onComplete = new SingleBroadcastReciver (filename, dm, mime,id);

			    context.RegisterReceiver(onComplete, new IntentFilter(
				    DownloadManager.ActionDownloadComplete));

            }

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
			
         
 
            
            //{
            Java.IO.File file = new Java.IO.File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads).AbsolutePath + "/" + filename);
            Toast.MakeText(context, "Nedlastning Klar", ToastLength.Short).Show();

            Intent intent1 = new Intent(Intent.ActionView);



            intent1.SetDataAndType(Uri.Parse(dm.GetUriForDownloadedFile(id).ToString()), "application/" +mime);
            //intent1.SetAction (Android.Content.Intent.ActionView);

            intent1.SetFlags(ActivityFlags.GrantReadUriPermission);
            intent1.SetFlags(ActivityFlags.NewTask);
            intent1.SetFlags(ActivityFlags.ClearWhenTaskReset);
            Xamarin.Forms.Forms.Context.StartActivity(intent1);
			//context.StartActivity(intent1);

            //}
  
        }

	}


}



