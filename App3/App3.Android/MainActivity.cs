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
using Android.Support.V4.App;
using Android.Support.V4.View;
using System.Globalization;
using System.Text.RegularExpressions;



namespace App3.Droid
{
    [Activity(Label = "Min Skatt", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class MainActivity : FragmentActivity
    {
        private XMLroot[] _items1;
		private XMLroot[] _items;
        public ListView ls;
        //FeedAdapter adapter;

        ImageButton inbox;
        ImageButton call;
        ImageButton maps;

        RelativeLayout OfflineView;
        ImageView OfflineImage;
        TextView OfflineText1;
        TextView OfflineText2;


        int CalendarChoice;

        private ViewPager _viewPager;
        private ViewPager _viewPager_RSS;




        public List<App3.Parser_xml.XMLroot> rssItems;



        protected override void OnResume()
        {
            base.OnResume();

            var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

            var activeConnection = connectivityManager.ActiveNetworkInfo;
            if ((activeConnection != null) && activeConnection.IsConnected)
            {
                OfflineView.Visibility = ViewStates.Gone;
                _viewPager.Visibility = ViewStates.Visible;
                _viewPager_RSS.Visibility = ViewStates.Visible;

                getxmlStringandParse();
                getxmlStringandParse2();

            }
            else
            {
                OfflineView.Visibility = ViewStates.Visible;
                _viewPager.Visibility = ViewStates.Gone;
                _viewPager_RSS.Visibility = ViewStates.Gone;

                OfflineText1.Text = "Kunne ikke opprette en tilkobling"; 
                OfflineText2.Text = "Vennligs kontroller at du har en internett tilkobling.";

                AlertDialog.Builder albuilder = new AlertDialog.Builder(this);
                albuilder.SetTitle("Nettverk");
                albuilder.SetMessage("Kunne ikke koble til internet");
                albuilder.SetCancelable(false);
                albuilder.SetPositiveButton("Ok", (object sender, DialogClickEventArgs e) =>
                { /* //Add a acton on ok button pressed//  */   });

                AlertDialog AlertBox = albuilder.Create();
                AlertBox.Show();




            }
					GC.Collect (GC.MaxGeneration);

        }



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

            _viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            _viewPager_RSS = FindViewById<ViewPager>(Resource.Id.viewPagerRSS);

            


            //ls = (ListView)FindViewById(Resource.Id.cListView);

            //ls.ItemClick += ls_ItemClick;
            OfflineView = FindViewById<RelativeLayout>(Resource.Id.OfflineView);
            OfflineImage = FindViewById<ImageView>(Resource.Id.OfflineImage);
            OfflineText1 = FindViewById<TextView>(Resource.Id.OfflineText1);
            OfflineText2 = FindViewById<TextView>(Resource.Id.OfflineText2);
             inbox = FindViewById<ImageButton>(Resource.Id.AltinnImageButton);
             call = FindViewById<ImageButton>(Resource.Id.CallImageButton);
			 maps = FindViewById<ImageButton>(Resource.Id.maps);
            //call.SetBackgroundColor(Android.Graphics.Color.Rgb(61, 147, 126));

            inbox.Click += inbox_Click;
			call.Click += call_Click;
			maps.Click += maps_Click;

            

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

            RSS_Fragment._itemsRSS = await SkatteetatenXML.XMLmain(SkatteetatenXML.Pressemeldinger_HTTP);

            if (RSS_Fragment._itemsRSS != null || RSS_Fragment._itemsRSS.Length != 0)
            {
                Array.Reverse(RSS_Fragment._itemsRSS);
                _viewPager_RSS.Adapter = new RSS_Fragment.RSS_Fragment_Adapter();
            }


                    //adapter = new FeedAdapter(this, _items);
                    //ls.SetAdapter(adapter);
                    //ls = new rssAdapter(this, _items);
        }

        #region Calendar RSS FEED

        /// <summary>
        /// This Void will retrieve the rss feed from skatteetaten.no,
        /// depending on user selection.
        /// </summary>

        public async void getxmlStringandParse2()
        {
			var xmlFeed = "";
            var prefs = Application.Context.GetSharedPreferences("Skatteetaten.perferences", FileCreationMode.Private);
            string CalendarSettings = prefs.GetString("Selected Calendar int", "");


            int output;
            bool result = int.TryParse(CalendarSettings, out output);
            if (result)
            {
                CalendarChoice = output;
            }
							
                if (CalendarChoice == 0)
				{ xmlFeed = SkatteetatenXML.Skattekalender_Personer; }
                else if (CalendarChoice == 1)
                { xmlFeed = SkatteetatenXML.Skattekalender_Bedrifter; }
                else
                { xmlFeed = SkatteetatenXML.Skattekalender_Personer; }
								




				XMLroot[] _temp = await SkatteetatenXML.XMLmain(xmlFeed);
                List<XMLroot> futureEvents = new List<XMLroot>();
                #region Finding Not Expired
                for (int i = 0; i < _temp.Count(); i++)
                {
                    #region Date Handling
                    
					DateTime contentdate = XmlConvert.ToDateTime(_temp[i].date);
                    
                    DateTime currentdate = DateTime.Now;
                    int Day = currentdate.Day;
                    int Month = currentdate.Month;
                    int Year = currentdate.Year;

	

                    #endregion


                    //Console.WriteLine("********************************************************************************************************");
										


                    if (contentdate.Year == Year)
                    {
                        if (contentdate.Month > Month)
                        {
                            //Add to not expired
                            //Console.WriteLine("NOT EXPIRED: [i] = " + i + " ITEM: " + _temp[i].title + "/" + _temp[i].link + "/" + _temp[i].date);
                            futureEvents.Add(new XMLroot { title = _temp[i].title, link = _temp[i].link, date = _temp[i].date });

                        }
                        else if (contentdate.Month == Month)
                        {
                            if (contentdate.Day > Day)
                            {
                                //Add to not expired
                                //Console.WriteLine("NOT EXPIRED: [i] = " + i + " ITEM: " + _temp[i].title + "/" + _temp[i].link + "/" + _temp[i].date);
                                futureEvents.Add(new XMLroot { title = _temp[i].title, link = _temp[i].link, date = _temp[i].date });
                            }
                            else if (contentdate.Day == Day)
                            {
                                //Add to not expired
                                //Console.WriteLine("NOT EXPIRED: [i] = " + i + " ITEM: " + _temp[i].title + "/" +  _temp[i].link + "/" + _temp[i].date);
                                futureEvents.Add(new XMLroot { title = _temp[i].title, link = _temp[i].link, date = _temp[i].date });
                            }
                        }

                    }
                    //Console.WriteLine("********************************************************************************************************");
                }
                #endregion

                
                XMLroot[] _green = futureEvents.ToArray();
                List<XMLroot> endlist = new List<XMLroot>();
                for (int i = 0; i < 4; i++)
                {
                    endlist.Add(new XMLroot { title = _green[i].title, link = _green[i].link, date = _green[i].date });
                }





                Event_Fragment._itemsEvents = endlist.ToArray();


                if (Event_Fragment._itemsEvents != null || Event_Fragment._itemsEvents.Length != 0)
                {
                    _viewPager.Adapter = new Event_Fragment.Event_Fragment_Adapter();
                }       

        }

        #endregion


        #region Bottom Buttons


        /// <summary>
        /// These are the click events for the bottom buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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

        #endregion


        

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