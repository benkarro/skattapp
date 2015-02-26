
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
				view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItemActivated2, null);


				view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position].Subject;
				view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = items[position].Status;
                //view.FindViewById<ImageView>(Android.Resource.Id.Icon).SetImageResource();//7w/df7qe/f7we
			return view;
		}
	}
}

