
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

namespace App3.Droid
{
	public class MessageAdapter : BaseAdapter {
		List<Messages>  items;
		Activity context;
		public MessageAdapter(Activity context, List<Messages> items) : base() {
			this.context = context;
			this.items = items;
		}
		public override long GetItemId(int position)
		{
			return position;
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}
//		public override string this[int position] {  
//			get { return items[position]; }
//		}
		public override int Count {
			get { return items.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Resource.Layout.inboxiconlist, null);

            /*
				view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position].Subject;
			view.FindViewById<TextView> (Android.Resource.Id.Text1).SetTextColor (Android.Graphics.Color.Black);
				view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = items[position].Status;
			view.FindViewById<TextView> (Android.Resource.Id.Text2).SetTextColor (Android.Graphics.Color.DarkGray);*/


            view.FindViewById<TextView>(Resource.Id.inbox_item_text1).Text = items[position].Subject;
            view.FindViewById<TextView>(Resource.Id.inbox_item_text1).SetTextColor(Android.Graphics.Color.Black);
            view.FindViewById<TextView>(Resource.Id.inbox_item_text2).Text = items[position].Status;
            view.FindViewById<TextView>(Resource.Id.inbox_item_text2).SetTextColor(Android.Graphics.Color.DarkGray);

            if (items[position].ServiceOwner == "Skatteetaten" || items[position].ServiceOwner == "skatteetaten" || items[position].ServiceOwner == "SKATTEETATEN") 
            {
                view.FindViewById<ImageView>(Resource.Id.inbox_item_icon).SetImageResource(Resource.Drawable.Skatteetaten);//7w/df7qe/f7we

            }
            else if (items[position].ServiceOwner == "Nav" || items[position].ServiceOwner == "nav" || items[position].ServiceOwner == "NAV")
            {
                view.FindViewById<ImageView>(Resource.Id.inbox_item_icon).SetImageResource(Resource.Drawable.Nav);
            }
            else if (items[position].ServiceOwner == "Altinn" || items[position].ServiceOwner == "altinn" || items[position].ServiceOwner == "ALTINN")
            {
                view.FindViewById<ImageView>(Resource.Id.inbox_item_icon).SetImageResource(Resource.Drawable.Altinn);
            }
            else if (items[position].ServiceOwner == "Lånekassen" || items[position].ServiceOwner == "lånekassen" || items[position].ServiceOwner == "LÅNEKASSEN" 
                || items[position].ServiceOwner == "Lanekassen" || items[position].ServiceOwner == "lanekassen" || items[position].ServiceOwner == "LANEKASSEN" )
            {
                view.FindViewById<ImageView>(Resource.Id.inbox_item_icon).SetImageResource(Resource.Drawable.Lanekassen);
            }


            else
            {
                view.FindViewById<ImageView>(Resource.Id.inbox_item_icon).SetImageResource(Resource.Drawable.Altinn);
            }




			return view;
		}
	}
}

