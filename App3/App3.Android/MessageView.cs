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
using App3.Parser;
using System.Threading.Tasks;




namespace App3.Droid
{
    [Activity(Label = "MessageView")]
	public class MessageView : ListActivity 
    {
        JSONparser JsonParser;
        HttpHelper2 HttpHelper2 = new HttpHelper2("https://www.altinn.no/api/my/messages");
		Task<String> Json;
		String JsonString;
		public List<Messages> messages;

		 


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Xamarin.Forms.Forms.Init(this,bundle);
            //laster ned JSON objekt

			getJsonStringandParse (HttpHelper2);




  
        }


		public async void getJsonStringandParse(HttpHelper2 http)
		{

			Json=  http.DownloadJson();
			JsonString =await Json;
			JsonParser = new JSONparser(JsonString);
			messages = JsonParser.messages;



			ListAdapter = new MessageAdapter(this, messages);


		}

		protected override void OnListItemClick (ListView l, View v, int position, long id)
		{
			String messagelink = messages [position]._links.self.href;
			var intent=new Intent(this, typeof(SingleMessageView));
			intent.PutExtra ("messagelink", messagelink);
			StartActivity (intent);



		}


    }
}