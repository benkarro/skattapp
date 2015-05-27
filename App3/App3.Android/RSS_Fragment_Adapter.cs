
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

namespace App3.Droid
{
	class RSS_Fragment_Adapter : PagerAdapter
	{
		public override int Count {
			get;
		}

		public override bool IsViewFromObject (View view, Java.Lang.Object @object)
		{
			throw new NotImplementedException ();
		}
	}

}

