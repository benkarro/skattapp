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
    [Activity(Label = "Inbox")]
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

			ActionBar actionBar = ActionBar;

			actionBar.SetHomeButtonEnabled(true);
			actionBar.SetDisplayHomeAsUpEnabled(true);



  
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
            string Header = messages[position].Subject.ToString();
            var intent = new Intent(this, typeof(SingleMessageView));
			intent.PutExtra ("messagelink", messagelink);
            intent.PutExtra("senderlabel", Header);

			StartActivity (intent);



		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{

			switch (item.ItemId)
			{
			case Android.Resource.Id.Home:
				Finish();
				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}
    }
}