
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

namespace App3.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	public class KontorActivity : Activity
	{
		public string _Kontor;
		public string _Åpningstid;
		public string _Telefon;
		public string _Telefaks;
		public string _Adresse;
		public string _Postboks;
		public string _Postkode1;
		public string _Postkode2;
		public string _Epost;
        public string _Latitude;
        public string _Longitude;


        public Double Latitude;
        public Double Longitude;


		public ImageButton Tid;
		public ImageButton	Call;
		public ImageButton Place;
		public ImageButton Postbks;
		public ImageButton Epost;


		public string _styleID;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			//IF Android version is lower than Lollipop
			if (((int)Android.OS.Build.VERSION.SdkInt) < 21)
			{
				//setSwitchColor();'


			}





			_Kontor = Intent.GetStringExtra ("Kontor");
			_Åpningstid = Intent.GetStringExtra ("Åpningstid");
            _Telefon = Intent.GetStringExtra("Telefon");
            _Telefaks = Intent.GetStringExtra("Telefaks");
			_Adresse = Intent.GetStringExtra ("Adresse");
			_Postboks = Intent.GetStringExtra ("Postboks");
			_Postkode1 = Intent.GetStringExtra ("Postkode1");
			_Postkode2 = Intent.GetStringExtra ("Postkode2");
			_Epost = Intent.GetStringExtra ("Epost");
            _Latitude = Intent.GetStringExtra("Latitude");
            _Longitude = Intent.GetStringExtra("Longitude");

			_styleID = Intent.GetStringExtra ("StyleID");



			int rid;
			bool res = (int.TryParse(_styleID, out rid));
			if (res) 
			{
				this.SetTheme (rid);

			}


			SetContentView (Resource.Layout.Kontor);


		


			if (_Latitude.Contains (",")) 
			{
				_Latitude = _Latitude.Replace (",", ".");
			}
			if (_Longitude.Contains (",")) 
			{
				_Longitude = _Longitude.Replace (",", ".");
			}








             Tid = FindViewById<ImageButton>(Resource.Id.kontor_tid_button);
			 Call = FindViewById<ImageButton>(Resource.Id.kontor_telefon_button);
             Place = FindViewById<ImageButton>(Resource.Id.kontor_adresse_button);
             Postbks = FindViewById<ImageButton>(Resource.Id.kontor_postboks_button);
             Epost = FindViewById<ImageButton>(Resource.Id.kontor_epost_button);

            Tid.Click += Tid_Click;
            Call.Click += Call_Click;
            Place.Click += Place_Click;
            Postbks.Click += Postbks_Click;
            Epost.Click += Epost_Click;
/*
			Android.Graphics.Color nColor = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(_primary_color));
			Android.Graphics.Color dColor = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(_primary_dark_color));


			Android.Graphics.Drawables.ColorDrawable cab = new Android.Graphics.Drawables.ColorDrawable(nColor);


			ActionBar.SetBackgroundDrawable(cab);*/

			this.Title = _Kontor;

			SetText ();
		}


        void Tid_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Åpningstid");
			builder.SetMessage("Åpningstiden for " + _Kontor + " er: \n" + _Åpningstid);
            builder.SetCancelable(false);
            builder.SetPositiveButton("Ok", (object sender1, DialogClickEventArgs e1) =>
            {
                
            });

            AlertDialog Alertbox = builder.Create();
            Alertbox.Show();
        }

        void Call_Click(object sender, EventArgs e)
        {
            int Nummer;
            bool res = (int.TryParse(_Telefon, out Nummer));
            if (res) { 
                var uri = Android.Net.Uri.Parse("tel:" + Nummer);
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            }
        }
        void Place_Click(object sender, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse("geo:" + _Latitude + "," +_Longitude + "?q=" + _Latitude + "," +_Longitude + "(" + _Kontor + ")");
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }
        void Postbks_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            //builder.SetTitle("Bekreftelse");
			builder.SetTitle("Postboks");
			builder.SetMessage("Postboksen til: \n" + _Kontor +"\n\n" + _Postboks + "\n" + _Postkode1);
            builder.SetCancelable(false);
            builder.SetPositiveButton("Ok", (object sender1, DialogClickEventArgs e1) =>
            {
                
            });
					
            AlertDialog Alertbox = builder.Create();
            Alertbox.Show();
        }

        void Epost_Click(object sender, EventArgs e)
        {
            if (_Epost != "" || _Epost != null)
            {
                if (_Epost.Contains("@"))
                {
                    var uri = Android.Net.Uri.Parse("mailto:" + _Epost);
                    var intent = new Intent(Intent.ActionView, uri);
                    StartActivity(intent);
                    
                }
            }
        }


		public void SetText() 
		{
            RelativeLayout ÅpningstidLayout = FindViewById<RelativeLayout>(Resource.Id.kontor_tid_holder);
            RelativeLayout TelefonLayout = FindViewById<RelativeLayout>(Resource.Id.kontor_telefon_holder);
            //RelativeLayout TelefaksLayout = FindViewById<RelativeLayout>(Resource.Id.kontor_telefaks_holder); TelefaksLayout.Visibility = ViewStates.Gone;
            RelativeLayout AdresseLayout = FindViewById<RelativeLayout>(Resource.Id.kontor_adresse_holder);
            RelativeLayout PostboksLayout = FindViewById<RelativeLayout>(Resource.Id.kontor_postboks_holder);
            RelativeLayout EpostLayout = FindViewById<RelativeLayout>(Resource.Id.kontor_epost_holder);

			TextView Åpningstid = FindViewById<TextView> (Resource.Id.kontor_tid_textview2);
			TextView Telefon = FindViewById<TextView> (Resource.Id.kontor_telefon_textview2);
			//TextView Telefaks = FindViewById<TextView> (Resource.Id.kontor_telefaks_textview2);
			TextView Adresse = FindViewById<TextView> (Resource.Id.kontor_adresse_textview2);
			TextView Adresse_Kode = FindViewById<TextView> (Resource.Id.kontor_adresse_textview3);
			TextView Postboks = FindViewById<TextView> (Resource.Id.kontor_postboks_textview2);
			TextView Postboks_Kode = FindViewById<TextView> (Resource.Id.kontor_postboks_textview3);
			TextView Epost = FindViewById<TextView> (Resource.Id.kontor_epost_textview2);


			if (_Åpningstid != "" ) 
			{
				Åpningstid.Text = _Åpningstid;
			}
			else { ÅpningstidLayout.Visibility = ViewStates.Gone; }



			if (_Telefon != "" ) 
			{
				Telefon.Text = _Telefon;
			}
			else { TelefonLayout.Visibility = ViewStates.Gone; }



			/*if (_Telefaks != "" || _Telefaks != null) 
			{
				Telefaks.Text = _Telefaks;
			}
			else { TelefaksLayout.Visibility = ViewStates.Gone; }*/



			if (_Adresse != "" ) 
			{
				Adresse.Text = _Adresse;
			}
			else { AdresseLayout.Visibility = ViewStates.Gone; }


			if (_Postkode2 != "") 
			{
				Adresse_Kode.Text = _Postkode2;
			}
			else { Adresse_Kode.Visibility = ViewStates.Gone; }



			if (_Postboks != "" ) 
			{
				Postboks.Text = _Postboks;
			}
			else { PostboksLayout.Visibility = ViewStates.Gone; }

			if (_Postkode1 != "") 
			{
				Postboks_Kode.Text = _Postkode1;
			}
			else { Postboks_Kode.Visibility = ViewStates.Gone; }



			if (_Epost != "") 
			{
				Epost.Text = _Epost;
			}
			else { EpostLayout.Visibility = ViewStates.Gone; }

		}




		protected override void OnDestroy ()
		{
			base.OnDestroy ();

			_Kontor = null;
			_Åpningstid = null;
			_Telefon = null;
			_Telefaks = null;
			_Adresse = null;
			_Postboks = null;
			_Postkode1 = null;
			_Postkode2 = null;
			_Epost = null;
			_Latitude = null;
			_Longitude = null;
			//SetTheme = null;
			this.Theme.Dispose();


			Tid.Click -= Tid_Click;
			Call.Click -= Call_Click;
			Place.Click -= Place_Click;
			Postbks.Click -= Postbks_Click;
			Epost.Click -= Epost_Click;


			Tid = null;
			Call = null;
			Place = null;
			Postbks = null;
			Epost = null;
			Finish ();
			GC.Collect (GC.MaxGeneration);

		}

    }


    

}

