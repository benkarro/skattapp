
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using App3.Parser_xml;
using System.Xml;
using System.Text.RegularExpressions;
using System.Globalization;
using Android.Graphics.Drawables;

namespace App3.Droid
{
	public class RSS_Fragment : Fragment
	{
		public static XMLroot[] _itemsRSS;
		private ViewPager _viewPager;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate (Resource.Layout.rssListView, container, false);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			_viewPager = view.FindViewById<ViewPager> (Resource.Id.viewPagerRSS);
			_viewPager.Adapter = new RSS_Fragment_Adapter ();
		}


		public class RSS_Fragment_Adapter : PagerAdapter 
		{
			public override int Count 
			{
				get { return _itemsRSS.Count (); }
			}


			public override bool IsViewFromObject (View view, Java.Lang.Object obj)
			{
				return view == obj;
			}

			public override Java.Lang.Object InstantiateItem (ViewGroup container, int position)
			{
				View view = LayoutInflater.From (container.Context).Inflate (Resource.Layout.rssListView, container, false);
				container.AddView (view);


				ImageView Icon = view.FindViewById<ImageView>(Resource.Id.rss_icon);
				TextView Publisher = view.FindViewById<TextView>(Resource.Id.rss_publisher);
				TextView Head = view.FindViewById<TextView>(Resource.Id.rss_head);
				TextView Text = view.FindViewById<TextView>(Resource.Id.rss_text);
				TextView Date = view.FindViewById<TextView>(Resource.Id.rss_date);






				DateTime th = XmlConvert.ToDateTime(_itemsRSS[position].date);

                if (_itemsRSS[position].title == null || _itemsRSS[position].title == "")
				{ Head.Text = "N/A"; }
				else
				{
                    if (_itemsRSS[position].title.Contains(">") || _itemsRSS[position].title.Contains("<"))
					{
                        string noHTML = Regex.Replace(_itemsRSS[position].title, @"<[^>]+>|&nbsp;", "").Trim();
						Head.Text = noHTML;
					}
					else
					{
                        Head.Text = _itemsRSS[position].title;
					}
				}

                if (_itemsRSS[position].description == null || _itemsRSS[position].description == "")
				{ Text.Text = "(N/A)"; }
				else
				{
                    if (_itemsRSS[position].description.Contains(">") || _itemsRSS[position].description.Contains("<"))
					{
                        string noHTML = Regex.Replace(_itemsRSS[position].description, @"<[^>]+>|&nbsp;", "").Trim();
						Text.Text = noHTML;
					}
					else
                    { Text.Text = _itemsRSS[position].description; }
				}

                if (_itemsRSS[position].date == null || _itemsRSS[position].date == "")
				{ Date.Text = "(N/A)"; }
				else
				{
                    DateTime pubdate = XmlConvert.ToDateTime(_itemsRSS[position].date);
					Date.Text = pubdate.ToShortDateString();
				}






				return view;
			}


            public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object obj)
            {
                container.RemoveView((View)obj);
            }

		}

	}











    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




    public class Event_Fragment : Fragment
    {
        public static XMLroot[] _itemsEvents;
        private ViewPager _viewPagerEvents;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.rssListView, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            _viewPagerEvents = view.FindViewById<ViewPager>(Resource.Id.viewPagerRSS);
            _viewPagerEvents.Adapter = new Event_Fragment_Adapter();
        }


        public class Event_Fragment_Adapter : PagerAdapter
        {
            public override int Count
            {
                get { return _itemsEvents.Count(); }
            }


            public override bool IsViewFromObject(View view, Java.Lang.Object obj)
            {
                return view == obj;
            }

            public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
            {
                View view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.calendarFragment, container, false);
                container.AddView(view);


                /*view.SetOnClickListener(new IDialogInterfaceOnClickListener()
                {

                });*/



                RelativeLayout rl = view.FindViewById<RelativeLayout>(Resource.Id.bigdate);
                TextView Icon = view.FindViewById<TextView>(Resource.Id.calendar_boxmonth);
                TextView Icon2 = view.FindViewById<TextView>(Resource.Id.calendar_boxday);
                TextView Head = view.FindViewById<TextView>(Resource.Id.calendar_item_text1);
                DateTime th = XmlConvert.ToDateTime(_itemsEvents[position].date);
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


                DateTime DateVerify = XmlConvert.ToDateTime(_itemsEvents[position].date);

                string HeadText = _itemsEvents[position].title;

                if (HeadText.Substring(0, 11).Contains(DateVerify.Date.ToShortDateString()))
                {
                    Head.Text = _itemsEvents[position].title.Replace(DateVerify.Date.ToShortDateString(), "").TrimStart();
                }
                else if (HeadText.Substring(0, 25).Contains(DateVerify.ToString())) //If text contains date + time format.
                {
                    Head.Text = _itemsEvents[position].title;
                }
                else
                {
                    Head.Text = _itemsEvents[position].title;
                }






                return view;
            }


            public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object obj)
            {
                container.RemoveView((View)obj);
            }

        }

    }














}

