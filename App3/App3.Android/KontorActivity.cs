
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

		public string _styleID;


		protected override void OnCreate (Bundle savedInstanceState)
		{

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

			_styleID = Intent.GetStringExtra ("StyleID");



			int rid;
			bool res = (int.TryParse(_styleID, out rid));
			if (res) 
			{
				SetTheme (rid);
			}


			SetContentView (Resource.Layout.Kontor);



			base.OnCreate (savedInstanceState);

            ImageButton Call = FindViewById<ImageButton>(Resource.Id.kontor_telefon_button);
            Call.Click += Call_Click;

/*
			Android.Graphics.Color nColor = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(_primary_color));
			Android.Graphics.Color dColor = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(_primary_dark_color));


			Android.Graphics.Drawables.ColorDrawable cab = new Android.Graphics.Drawables.ColorDrawable(nColor);


			ActionBar.SetBackgroundDrawable(cab);*/

			this.Title = _Kontor;

			SetText ();
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

		public void SetText() 
		{
            RelativeLayout ÅpningstidLayout = FindViewById<RelativeLayout>(Resource.Id.kontor_tid_holder);
            RelativeLayout TelefonLayout = FindViewById<RelativeLayout>(Resource.Id.kontor_telefon_holder);
            RelativeLayout TelefaksLayout = FindViewById<RelativeLayout>(Resource.Id.kontor_telefaks_holder);
            RelativeLayout AdresseLayout = FindViewById<RelativeLayout>(Resource.Id.kontor_adresse_holder);
            RelativeLayout PostboksLayout = FindViewById<RelativeLayout>(Resource.Id.kontor_postboks_holder);
            RelativeLayout EpostLayout = FindViewById<RelativeLayout>(Resource.Id.kontor_epost_holder);

			TextView Åpningstid = FindViewById<TextView> (Resource.Id.kontor_tid_textview2);
			TextView Telefon = FindViewById<TextView> (Resource.Id.kontor_telefon_textview2);
			TextView Telefaks = FindViewById<TextView> (Resource.Id.kontor_telefaks_textview2);
			TextView Adresse = FindViewById<TextView> (Resource.Id.kontor_adresse_textview2);
			TextView Adresse_Kode = FindViewById<TextView> (Resource.Id.kontor_adresse_textview3);
			TextView Postboks = FindViewById<TextView> (Resource.Id.kontor_postboks_textview2);
			TextView Postboks_Kode = FindViewById<TextView> (Resource.Id.kontor_postboks_textview3);
			TextView Epost = FindViewById<TextView> (Resource.Id.kontor_epost_textview2);


			if (_Åpningstid != "" || _Åpningstid != null) 
			{
				Åpningstid.Text = _Åpningstid;
			}
			else { ÅpningstidLayout.Visibility = ViewStates.Gone; }



			if (_Telefon != "" || _Telefon != null) 
			{
				Telefon.Text = _Telefon;
			}
			else { TelefonLayout.Visibility = ViewStates.Gone; }



			if (_Telefaks != "" || _Telefaks != null) 
			{
				Telefaks.Text = _Telefaks;
			}
			else { TelefaksLayout.Visibility = ViewStates.Gone; }



			if (_Adresse != "" || _Adresse != null) 
			{
				Adresse.Text = _Adresse;
			}
			else { AdresseLayout.Visibility = ViewStates.Gone; }


			if (_Postkode2 != "" || _Postkode2 != null) 
			{
				Adresse_Kode.Text = _Postkode2;
			}
			else { Adresse_Kode.Visibility = ViewStates.Gone; }



			if (_Postboks != "" || _Postboks != null) 
			{
				Postboks.Text = _Postboks;
			}
			else { PostboksLayout.Visibility = ViewStates.Gone; }

			if (_Postkode1 != "" || _Postkode1 != null) 
			{
				Postboks_Kode.Text = _Postkode1;
			}
			else { Postboks_Kode.Visibility = ViewStates.Gone; }



			if (_Epost != "" || _Epost != null) 
			{
				Epost.Text = _Epost;
			}
			else { EpostLayout.Visibility = ViewStates.Gone; }

		}


        protected override void OnStop()
        {
            Finish();
            base.OnStop();
        }

    }


    

}

