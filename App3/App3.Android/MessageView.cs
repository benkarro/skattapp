using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using App3.Parser;
using System.Threading.Tasks;




namespace App3.Droid
{
    [Activity(Label = "Inbox", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	public class MessageView : Activity 
    {

        JSONparser JsonParser;
        HttpHelper2 HttpHelper2 = new HttpHelper2("https://www.altinn.no/api/my/messages");
		Task<String> Json;
		String JsonString;
		public List<Messages> messages;

        ListView InboxList;
        MessageAdapter InboxAdapter;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Xamarin.Forms.Forms.Init(this,bundle);
            //laster ned JSON objekt

			getJsonStringandParse (HttpHelper2);

			ActionBar actionBar = ActionBar;

			actionBar.SetHomeButtonEnabled(true);
			actionBar.SetDisplayHomeAsUpEnabled(true);
            //
            SetContentView(Resource.Layout.Inbox);

            InboxList = FindViewById<ListView>(Resource.Id.InboxList);

			InboxList.ItemClick += InboxList_ItemClick;
        }

        void InboxList_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {
			string messagelink = messages [e.Position]._links.self.href;
			string Header = messages [e.Position].Subject.ToString ();

			var intent = new Intent (this, typeof(SingleMessageView));
			intent.PutExtra ("messagelink", messagelink);
			intent.PutExtra("senderlabel", Header);

			StartActivity (intent);


        }



		public async void getJsonStringandParse(HttpHelper2 http)
		{
            



			Json=  http.DownloadJson();
			JsonString =await Json;
			JsonParser = new JSONparser(JsonString);
			messages = JsonParser.messages;



            InboxAdapter = new MessageAdapter(this, messages);
            InboxList.SetAdapter(InboxAdapter);




			//ListAdapter = new MessageAdapter(this, messages);

		}

		/*protected override void OnListItemClick (ListView l, View v, int position, long id)
		{
			String messagelink = messages [position]._links.self.href;
            string Header = messages[position].Subject.ToString();
            var intent = new Intent(this, typeof(SingleMessageView));
			intent.PutExtra ("messagelink", messagelink);
            intent.PutExtra("senderlabel", Header);

			StartActivity (intent);



		}*/


		public void SignOut() 
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (this);
			builder.SetTitle ("Bekreftelse");
			builder.SetMessage ("Er du sikker på at du vil logge ut?");
			builder.SetCancelable (false);
			builder.SetPositiveButton("Ja", (object sender, DialogClickEventArgs e) => {
				CookieManager.Instance.RemoveSessionCookie();
				Intent MainActivity_i = new Intent(this, typeof(MainActivity));
				MainActivity_i.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

				StartActivity(MainActivity_i);
				Finish();
			});
			builder.SetNegativeButton ("Nei", (object sender, DialogClickEventArgs e) => {
				Toast.MakeText(this, "Avbrutt", ToastLength.Short).Show();
			});

			AlertDialog Alertbox = builder.Create ();
			Alertbox.Show ();
		}





	    public override bool OnOptionsItemSelected (IMenuItem item)
		{

			switch (item.ItemId)
			{
			case Android.Resource.Id.Home:
				Finish();
				return true;


			case Resource.Id.logout:
				
				SignOut ();


				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}

		void ThisButtonView_DismissEvent (object sender, EventArgs e)
		{

		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater inflater = MenuInflater;
			inflater.Inflate (Resource.Menu.actionbar_buttons, menu);

			return base.OnCreateOptionsMenu (menu);
		}
    }
}