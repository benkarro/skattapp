
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
    public class FeedAdapter : BaseAdapter<XMLroot>
    {
        public Android.Content.Res.Resources res;
        private XMLroot[] _items;
        Activity _context;

        public FeedAdapter(Activity context, XMLroot[] _items)
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
                view = _context.LayoutInflater.Inflate(Resource.Layout.rssListView, null);
            }
            //ID's
            //rss_icon //Drawable
            //rss_publisher
            //rss_head
            //rss_text
            //rss_date


            ImageView Icon = view.FindViewById<ImageView>(Resource.Id.rss_icon);
            TextView Publisher = view.FindViewById<TextView>(Resource.Id.rss_publisher);
            TextView Head = view.FindViewById<TextView>(Resource.Id.rss_head);
            TextView Text = view.FindViewById<TextView>(Resource.Id.rss_text);
            TextView Date = view.FindViewById<TextView>(Resource.Id.rss_date);



            DateTime th = XmlConvert.ToDateTime(_items[position].date);

            if (_items[position].title == null || _items[position].title == "") 
            { Head.Text = "N/A"; } else
            {
                if (_items[position].title.Contains(">") || _items[position].title.Contains("<"))
                {
                    string noHTML = Regex.Replace(_items[position].title, @"<[^>]+>|&nbsp;", "").Trim();
                    Head.Text = noHTML;
                } else {
                    Head.Text = _items[position].title;
                }
            }

            if (_items[position].description == null || _items[position].description == "")
            { Text.Text = "(N/A)"; } else
            {
                if (_items[position].description.Contains(">") || _items[position].description.Contains("<"))
                {
                    string noHTML = Regex.Replace(_items[position].description, @"<[^>]+>|&nbsp;", "").Trim();
                    Text.Text = noHTML;
                } else 
            { Text.Text = _items[position].description; } 
            }

            if (_items[position].date == null || _items[position].date == "")
            { Date.Text = "(N/A)"; } else
            {
                DateTime pubdate = XmlConvert.ToDateTime(_items[position].date);
                Date.Text = pubdate.ToShortDateString();
            }

            return view;
        }






    

















    }
}