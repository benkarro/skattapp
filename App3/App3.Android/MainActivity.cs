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



namespace App3.Droid
{
    [Activity(Label = "SkatteInfo", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //Superlys..
            //ColorDrawable cab = new ColorDrawable(Android.Graphics.Color.Rgb(141, 217, 181));
            
            //M�rk..
            //ColorDrawable cab = new ColorDrawable(Android.Graphics.Color.Rgb(154, 140, 130));
         //   ColorDrawable cab = new ColorDrawable(Android.Graphics.Color.Rgb(61, 147, 126));
            
          //  ActionBar.SetBackgroundDrawable(cab);
            // Create your application here

            SetContentView(Resource.Layout.Main);
            ImageButton inbox = FindViewById<ImageButton>(Resource.Id.AltinnImageButton);
            inbox.Click += inbox_Click;

			ImageButton maps = FindViewById<ImageButton>(Resource.Id.maps);
			maps.Click += maps_Click;
        }

        void inbox_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(AltinnActivity));
        }

		void maps_Click(object sender, EventArgs e)
		{
			StartActivity(typeof(MapsActivity));
		}

        

        

    }
}