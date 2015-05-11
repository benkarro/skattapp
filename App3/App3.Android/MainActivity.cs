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
        private XMLroot[] _items;
        public ListView ls;
        FeedAdapter adapter;

        ImageButton inbox;
        ImageButton call;
        ImageButton maps;
        int CalendarChoice;

        private ViewPager _viewPager;
        private ViewPager _viewPager_RSS;




        public List<App3.Parser_xml.XMLroot> rssItems;

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

             inbox = FindViewById<ImageButton>(Resource.Id.AltinnImageButton);
             call = FindViewById<ImageButton>(Resource.Id.CallImageButton);
			 maps = FindViewById<ImageButton>(Resource.Id.maps);
            //call.SetBackgroundColor(Android.Graphics.Color.Rgb(61, 147, 126));

            inbox.Click += inbox_Click;
			call.Click += call_Click;
			maps.Click += maps_Click;

            getxmlStringandParse();
            getxmlStringandParse2();
            


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

                    RSSFragmentAdapter._RSSitems = _items;

                    if (RSSFragmentAdapter._RSSitems != null || RSSFragmentAdapter._RSSitems.Length != 0)
                    {
                        
                        _viewPager_RSS.Adapter = new RSSFragmentAdapter(SupportFragmentManager);
                    }
                    //adapter = new FeedAdapter(this, _items);
                    //ls.SetAdapter(adapter);
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

        public async void getxmlStringandParse2()
        {
            var prefs = Application.Context.GetSharedPreferences("Skatteetaten.perferences", FileCreationMode.Private);
            string CalendarSettings = prefs.GetString("Selected Calendar int", "");


            int output;
            bool result = int.TryParse(CalendarSettings, out output);
            if (result)
            {
                CalendarChoice = output;
            }

            //CalendarChoice = (int.Parse(CalendarSettings));


            /*Xml = http.DownloadXML();
            XmlString = await Xml;
            XmlParser = new XMLparser(XmlString);*/
            using (var client = new HttpClient())
            {
                var xmlFeed = "";
                if (CalendarChoice == 0)
                {
                    xmlFeed = await client.GetStringAsync("http://www.skatteetaten.no/no/Stottemenyer/RSS-feeder/Skattekalender-for-personer/");
                }
                else if (CalendarChoice == 1)
                {
                    xmlFeed = await client.GetStringAsync("http://www.skatteetaten.no/no/Stottemenyer/RSS-feeder/Kalender-for-bedrifter-og-organisasjoner/");
                }
                else
                {
                    xmlFeed = await client.GetStringAsync("http://www.skatteetaten.no/no/Stottemenyer/RSS-feeder/Skattekalender-for-personer/");
                }


                //var xmlFeed = await client.GetStringAsync("http://www.skatteetaten.no/no/Stottemenyer/RSS-feeder/Kalender-for-bedrifter-og-organisasjoner/");
                //
                XMLroot[] _temp;
                var DOC = XDocument.Parse(xmlFeed);
                XNamespace dc = "http://purl.org/dc/elements/1.1/";

                _temp = (from item in DOC.Descendants("item")
                          select new XMLroot
                          {
                              title = item.Element("title").Value,
                              link = item.Element("link").Value,
                              date = item.Element(dc + "date").Value
                          }).ToArray();
                //_items.Reverse();


                /*for (int i = 0; i < _temp.Count(); i++)
                {
                    XMLroot[] _con;
                    DateTime thisdate = XmlConvert.ToDateTime(_temp[i].date);
                    DateTime currentdate = DateTime.Now;
                    int Day = currentdate.Day;
                    int Month = currentdate.Month;
                    int Year = currentdate.Year;




                    if (thisdate.Day > Day)
                    {
                        
                    }



                }*/

                CalendarFragmentAdapter._calendarItems = _temp;



                if (CalendarFragmentAdapter._calendarItems != null || CalendarFragmentAdapter._calendarItems.Length != 0)
                {
                    Array.Reverse(CalendarFragmentAdapter._calendarItems);
                    _viewPager.Adapter = new CalendarFragmentAdapter(SupportFragmentManager);
                }
                //ls = new rssAdapter(this, _items);
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


    public class CalendarFragmentAdapter : FragmentPagerAdapter
    {
        public static XMLroot[] _calendarItems;
        public static int _pos = 0;
        //public static int _selectedIitem;
        public CalendarFragmentAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm) { }

        public override int Count
        {
            get { return _calendarItems.Count(); }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
             return new CalendarFragment();
        }

        public override long GetItemId(int position)
        {
            _pos = position;
            return position;
        }
        

    } 

    public class CalendarFragment : Android.Support.V4.App.Fragment
    {


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.calendarFragment, container, false);




            RelativeLayout rl = view.FindViewById<RelativeLayout>(Resource.Id.bigdate);

            TextView Icon = view.FindViewById<TextView>(Resource.Id.calendar_boxmonth);
            TextView Icon2 = view.FindViewById<TextView>(Resource.Id.calendar_boxday);


            TextView Head = view.FindViewById<TextView>(Resource.Id.calendar_item_text1);


            DateTime th = XmlConvert.ToDateTime(CalendarFragmentAdapter._calendarItems[CalendarFragmentAdapter._pos].date);

            DateTime th2 = DateTime.Now;

            int Day = th.Day;
            int Month = th.Month;
            int Year = th.Year;

            string DisplayDate = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month).ToUpper().Substring(0, 3);
            #region Try
            try
            {

                if (th.Year == th2.Year)
                {
                    #region Current Month
                    if (th.Month == th2.Month)
                    {
                        #region Current Day
                        if (th.Day == th2.Day)
                        {
                            string cvCo = Application.Context.GetString(Resource.Color.Secondary5);
                            Android.Graphics.Color thCol = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(cvCo));
                            rl.Background = new ColorDrawable(thCol);
                        }
                        #endregion
                        #region Future
                        if (th.Day > th2.Day)
                        {
                            string cvCo = Application.Context.GetString(Resource.Color.Secondary1);
                            Android.Graphics.Color thCol = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(cvCo));
                            rl.Background = new ColorDrawable(thCol);
                        }
                        #endregion
                        #region Past
                        if (th.Day < th2.Day)
                        {
                            string cvCo = Application.Context.GetString(Resource.Color.Primary3);
                            Android.Graphics.Color thCol = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(cvCo));
                            rl.Background = new ColorDrawable(thCol);
                        }
                        #endregion
                    }
                    #endregion
                    #region Future Month
                    else if (th.Month > th2.Month)
                    {
                        string cvCo = Application.Context.GetString(Resource.Color.Secondary1);
                        Android.Graphics.Color thCol = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(cvCo));
                        rl.Background = new ColorDrawable(thCol);
                    }
                    #endregion
                    #region Past Month
                    else if (th.Month < th2.Month)
                    {
                        string cvCo = Application.Context.GetString(Resource.Color.Primary3);
                        Android.Graphics.Color thCol = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(cvCo));
                        rl.Background = new ColorDrawable(thCol);
                    }
                    #endregion
                }
                else if (th.Year > th2.Year)
                {
                    string cvCo = Application.Context.GetString(Resource.Color.Secondary1);
                    Android.Graphics.Color thCol = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(cvCo));
                    rl.Background = new ColorDrawable(thCol);
                }
                else if (th.Year < th2.Year)
                {
                    string cvCo = Application.Context.GetString(Resource.Color.Primary3);
                    Android.Graphics.Color thCol = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(cvCo));
                    rl.Background = new ColorDrawable(thCol);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Something Went Wrong When Manually Setting Color On The Layout Contating Date And Month...");

            }
            #endregion


            Icon.Text = DisplayDate;
            Icon2.Text = Day.ToString();


            DateTime DateVerify = XmlConvert.ToDateTime(CalendarFragmentAdapter._calendarItems[CalendarFragmentAdapter._pos].date);

            string HeadText = CalendarFragmentAdapter._calendarItems[CalendarFragmentAdapter._pos].title;

            if (HeadText.Substring(0, 11).Contains(DateVerify.Date.ToShortDateString()))
            {
                Head.Text = CalendarFragmentAdapter._calendarItems[CalendarFragmentAdapter._pos].title.Replace(DateVerify.Date.ToShortDateString(), "").TrimStart();
            }
            else if (HeadText.Substring(0, 25).Contains(DateVerify.ToString())) //If text contains date + time format.
            {
                Head.Text = CalendarFragmentAdapter._calendarItems[CalendarFragmentAdapter._pos].title;
            }
            else
            {
                Head.Text = CalendarFragmentAdapter._calendarItems[CalendarFragmentAdapter._pos].title;
            }


            return view;
        }
    }


    public class RSSFragment : Android.Support.V4.App.Fragment 
    {
	    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) 
	    {
		    var view = inflater.Inflate(Resource.Layout.rssListView, container, false);
		    //Add bind code

            ImageView Icon = view.FindViewById<ImageView>(Resource.Id.rss_icon);
            TextView Publisher = view.FindViewById<TextView>(Resource.Id.rss_publisher);
            TextView Head = view.FindViewById<TextView>(Resource.Id.rss_head);
            TextView Text = view.FindViewById<TextView>(Resource.Id.rss_text);
            TextView Date = view.FindViewById<TextView>(Resource.Id.rss_date);



            DateTime th = XmlConvert.ToDateTime(RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].date);

            if (RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].title == null || RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].title == "")
            { Head.Text = "N/A"; }
            else
            {
                if (RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].title.Contains(">") || RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].title.Contains("<"))
                {
                    string noHTML = Regex.Replace(RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].title, @"<[^>]+>|&nbsp;", "").Trim();
                    Head.Text = noHTML;
                }
                else
                {
                    Head.Text = RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].title;
                }
            }

            if (RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].description == null || RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].description == "")
            { Text.Text = "(N/A)"; }
            else
            {
                if (RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].description.Contains(">") || RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].description.Contains("<"))
                {
                    string noHTML = Regex.Replace(RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].description, @"<[^>]+>|&nbsp;", "").Trim();
                    Text.Text = noHTML;
                }
                else
                { Text.Text = RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].description; }
            }

            if (RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].date == null || RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].date == "")
            { Date.Text = "(N/A)"; }
            else
            {
                DateTime pubdate = XmlConvert.ToDateTime(RSSFragmentAdapter._RSSitems[RSSFragmentAdapter._pos].date);
                Date.Text = pubdate.ToShortDateString();
            }





		    return view;
	    }
    }



    public class RSSFragmentAdapter : FragmentPagerAdapter
    {
        public static XMLroot[] _RSSitems;
        public static int _pos;

        public RSSFragmentAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm) { }

        public override int Count
        {
            get { return _RSSitems.Count(); }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return new RSSFragment();
        }

        public override long GetItemId(int position)
        {
            _pos = position;
            return base.GetItemId(position);
        }


    }



}