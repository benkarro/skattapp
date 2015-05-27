
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

				if (_itemsRSS[position].title == null || RSSFragmentAdapter._RSSitems[position].title == "")
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
}

