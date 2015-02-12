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


namespace App3.Droid
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here

            SetContentView(Resource.Layout.Main);
            ImageButton inbox = FindViewById<ImageButton>(Resource.Id.AltinnImageButton);
            inbox.Click += inbox_Click;
        }

        void inbox_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(AltinnActivity));
        }

        

        

    }
}