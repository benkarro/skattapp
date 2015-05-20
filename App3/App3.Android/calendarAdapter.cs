
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
using App3.Parser_xml;
using System.Xml;
using System.Text.RegularExpressions;
using System.Globalization;
using Android.Graphics.Drawables.Shapes;
using Android.Graphics.Drawables;

namespace App3.Droid
{
    public class calendarAdapter : BaseAdapter<XMLroot>
    {
        public Android.Content.Res.Resources res;
        private XMLroot[] _items;
        Activity _context;

        public calendarAdapter(Activity context, XMLroot[] _items)
            : base()
        {
            this._context = context;
            this._items = _items;
        }

        public override XMLroot this[int position]
        {
            get { return _items[position]; }
        }

        public override int Count
        {
            get { return _items.Count(); }
        }

        public override long GetItemId(int position)
        {
            return position;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null) 
            { 
                view = _context.LayoutInflater.Inflate(Resource.Layout.calendarListView, null);
            }
            //ID's
            //rss_icon //Drawable
            //rss_publisher
            //rss_head
            //rss_text
            //rss_date

            RelativeLayout rl = view.FindViewById<RelativeLayout>(Resource.Id.bigdate);

            TextView Icon = view.FindViewById<TextView>(Resource.Id.calendar_boxmonth);
            TextView Icon2 = view.FindViewById<TextView>(Resource.Id.calendar_boxday);


            TextView Head = view.FindViewById<TextView>(Resource.Id.calendar_item_text1);
            TextView Date = view.FindViewById<TextView>(Resource.Id.calendar_item_text2);


            DateTime th = XmlConvert.ToDateTime(_items[position].date);

            DateTime th2 = DateTime.Now;

            int Day = th.Day;
            int Month = th.Month;
            int Year = th.Year;

            string DisplayDate = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month).ToUpper().Substring(0,3);

            try {

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



            Icon.Text = DisplayDate;
            Icon2.Text = Day.ToString();


            DateTime DateVerify = XmlConvert.ToDateTime(_items[position].date);
			//string cDateVerify = DateVerify.Day + "." + DateVerify.Month + "." + DateVerify.Year;

            string HeadText = _items[position].title;

			/*if (HeadText.Substring (0, 11).Contains (DateVerify.Date.ToShortDateString ())) {
				Head.Text = _items [position].title.Replace (DateVerify.Date.ToShortDateString (), "").TrimStart ();
			} */


			string DateTimeCheck = HeadText.Substring (0, 11);
			DateTime ParseResult;

			bool res = DateTime.TryParse(DateTimeCheck, out ParseResult);
			if (res) {
				Head.Text = _items [position].title.Replace (DateTimeCheck, "").TrimStart ();
			} else {
				Head.Text = _items [position].title;
			}








            

            #region Sets Date Time With only Date

            DateTime pubdate = XmlConvert.ToDateTime(_items[position].date);
            Date.Text = pubdate.ToShortDateString();

            #endregion

            return view;
        }






    }
}
