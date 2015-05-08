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
using Android.Graphics.Drawables;
using Android.Graphics;
using System.Net.Http;
using System.Xml.Linq;
using System.Xml;
using App3.Parser_xml;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Android.Net;
using Android.Support.V4.View;



namespace App3.Droid
{
    [Activity(Label = "Min Skatt", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class MainActivity : Activity
    {
        private XMLroot[] _items;
        public ListView ls;
        FeedAdapter adapter;

        ImageButton inbox;
        ImageButton call;
        ImageButton maps;

        private ViewPager _viewPager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //Superlys..
            //ColorDrawable cab = new ColorDrawable(Android.Graphics.Color.Rgb(141, 217, 181));
            
            //Mørk..
            //ColorDrawable cab = new ColorDrawable(Android.Graphics.Color.Rgb(154, 140, 130));
         //   ColorDrawable cab = new ColorDrawable(Android.Graphics.Color.Rgb(61, 147, 126));
            
          //  ActionBar.SetBackgroundDrawable(cab);
            // Create your application here

            SetContentView(Resource.Layout.Main);

            //var listviewAdapter = FindViewById(Resource.Id.cListView);

            ls = (ListView)FindViewById(Resource.Id.cListView);
            _viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            //_viewPager.Adapter = new calendarAdapter;
            //_viewPager.Adapter = new calendarAdapter(this, _items);



            ls.ItemClick += ls_ItemClick;

             inbox = FindViewById<ImageButton>(Resource.Id.AltinnImageButton);
             call = FindViewById<ImageButton>(Resource.Id.CallImageButton);
			 maps = FindViewById<ImageButton>(Resource.Id.maps);
            //call.SetBackgroundColor(Android.Graphics.Color.Rgb(61, 147, 126));

            inbox.Click += inbox_Click;
			call.Click += call_Click;
			maps.Click += maps_Click;

            getxmlStringandParse();
            


        }

        void ls_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            string title;
            string link;
            string subject;

            if (_items[e.Position].title != null)
            { title = _items[e.Position].title.ToString(); }

            if (_items[e.Position].link != null)
            { link = _items[e.Position].link.ToString(); }
            else
            { return; }

            if (_items[e.Position].description != null)
            { subject = _items[e.Position].description.ToString(); }

            Android.Net.Uri uri = Android.Net.Uri.Parse(link);

            Intent intent = new Intent(Intent.ActionView);
            intent.SetData(uri);
            Intent chooser = Intent.CreateChooser(intent, "Åpne linken med");

            this.StartActivity(chooser);

        }

        public async void getxmlStringandParse()
        {

            var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

            var activeConnection = connectivityManager.ActiveNetworkInfo;
            if ((activeConnection != null) && activeConnection.IsConnected)
            {
                
            
                /*Xml = http.DownloadXML();
                XmlString = await Xml;
                XmlParser = new XMLparser(XmlString);*/
                using (var client = new HttpClient()) { 
                    var xmlFeed = await client.GetStringAsync("http://www.skatteetaten.no/no/Stottemenyer/RSS-feeder/Pressemeldinger/");
                    //var xmlFeed = await client.GetStringAsync("http://www.skatteetaten.no/no/Stottemenyer/RSS-feeder/Kalender-for-bedrifter-og-organisasjoner/");
                    //

                    var DOC = XDocument.Parse(xmlFeed);
                    XNamespace dc = "http://purl.org/dc/elements/1.1/";

                    _items = (from item in DOC.Descendants("item")
                              select new XMLroot
                              {
                                  title = item.Element("title").Value,
                                  link = item.Element("link").Value,
                                  description = item.Element("description").Value,
                                  date = item.Element(dc + "date").Value
                              }).ToArray();
                    //_items.Reverse();
                    Array.Reverse(_items);
                    adapter = new FeedAdapter(this, _items);
                    ls.SetAdapter(adapter);
                    //ls = new rssAdapter(this, _items);
                }

            }
            else
            {
                AlertDialog.Builder albuilder = new AlertDialog.Builder(this);
                albuilder.SetTitle("Nettverk");
                albuilder.SetMessage("Kunne ikke koble til internet");
                albuilder.SetCancelable(false);
                albuilder.SetPositiveButton("Ok", (object sender, DialogClickEventArgs e) =>
                {


                 

                });

                AlertDialog AlertBox = albuilder.Create();
                AlertBox.Show();

            }

        }


        void call_Click(object sender, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse("tel:" + App3.Resources.Strings.SkatteopplysningenTLF);
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }

        void inbox_Click(object sender, EventArgs e)
        {
            var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

            var activeConnection = connectivityManager.ActiveNetworkInfo;
            if ((activeConnection != null) && activeConnection.IsConnected)
            {
                StartActivity(typeof(AltinnActivity));
            }
            else
            {
                AlertDialog.Builder albuilder = new AlertDialog.Builder(this);
                albuilder.SetTitle("Nettverk");
                albuilder.SetMessage("Kunne ikke hente data");
                albuilder.SetCancelable(false);
                albuilder.SetPositiveButton("Ok", (object sender2, DialogClickEventArgs e2) =>
                { });

                AlertDialog AlertBox = albuilder.Create();
                AlertBox.Show();
            }
        }

		void maps_Click(object sender, EventArgs e)
		{
			StartActivity(typeof(MapsActivity));
		}


        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Resource.Id.calendar:

                    
                    var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

                    var activeConnection = connectivityManager.ActiveNetworkInfo;
                    if ((activeConnection != null) && activeConnection.IsConnected)
                    {
                        Intent i = new Intent(this, typeof(CalendarActivity));

                        StartActivity(i);
                    }
                    else
                    {
                        AlertDialog.Builder albuilder = new AlertDialog.Builder(this);
                        albuilder.SetTitle("Nettverk");
                        albuilder.SetMessage("Kunne ikke hente data");
                        albuilder.SetCancelable(false);
                        albuilder.SetPositiveButton("Ok", (object sender2, DialogClickEventArgs e2) =>
                        { });

                        AlertDialog AlertBox = albuilder.Create();
                        AlertBox.Show();
                    }
                    
                    
				    //Finish();

                    return true;

                case Resource.Id.settings:

                    Intent i2 = new Intent(this, typeof(SettingsActivity));

                    StartActivity(i2);
                    //Finish(); //Finish closes the main intent
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.actionbar_buttons_Main, menu);

            return base.OnCreateOptionsMenu(menu);
        }
        

    }
}