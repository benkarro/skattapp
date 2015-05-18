using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Views;
using Android.Widget;
using App3.Resources;


namespace App3.Droid
{
	[Activity ()]			
	public class MapsActivity : Activity, IOnMapReadyCallback, GoogleMap.IOnInfoWindowClickListener, GoogleMap.IOnMyLocationButtonClickListener
	{

		private GoogleMap mMap;

        public static KontorInfo[] KontorInformasjon;

        public static List<string> ValgteKontorer = new List<string>();
        public static List<string> ValgteSwitcher = new List<string>();


        Switch Kemnern;
        Switch Skatteoppkrever;
        Switch Kommunekasserer;
        Switch Skattekontorer;
        Switch Sentralskattekontoret;
        Switch Oljeskattekontoret;
        Switch Skattedirektoratet;
        Switch Servicepartner;
        Switch SITSBrukersenter;
        Switch Registerinfo;




		//private  List<String> kontorer;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "Maps" layout resource
			SetContentView (Resource.Layout.Maps);


            
            
            
            SetUpMap ();

		}

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {

            if (keyCode == Keycode.Menu)
            {
                showSelection();
                return true;
            }

            return base.OnKeyUp(keyCode, e);
        }


		private void SetUpMap (){
		if (mMap == null){
				FragmentManager.FindFragmentById<MapFragment> (Resource.Id.map).GetMapAsync(this);
			}
		}



		public void OnMapReady(GoogleMap googleMap)
		{

			mMap = googleMap;
			mMap.SetOnInfoWindowClickListener (this);
			mMap.MyLocationEnabled = true;
			mMap.SetOnMyLocationButtonClickListener (this);
				



		
			LatLng norge = new LatLng (64.5783089, 17.888237);
			LatLng posission2 = new LatLng (40, -73);


			CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom (norge, 4);
			mMap.MoveCamera (camera);



			//foreach(string element in kontorer){
			//	mMap.AddMarker (new MarkerOptions ()
			//		.SetPosition (posission2)
			//		.SetTitle ("yo")
			//		.SetSnippet("aka big town")
			//	);
				
			//}


            /*MarkerOptions hq = new MarkerOptions();
            hq.SetPosition(new LatLng (59.9145366, 10.802480599999967));
            hq.SetTitle("Skattedirektoratet og SITS");
            hq.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));
            mMap.AddMarker(hq);*/


            //mMap.Clear();

            SetMarkers();
		}


        public void SetMarkers()
        {
            mMap.Clear();

            KontorInformasjon = Kontorer.getKontorer(ValgteKontorer).ToArray();
            //Console.WriteLine(KontorInformasjon.Length);

            for (int i = 0; i < KontorInformasjon.Length; i++)
            {
                MarkerOptions offices = new MarkerOptions();
                offices.SetPosition(new LatLng(KontorInformasjon[i].Latitude, KontorInformasjon[i].Longitude));
                offices.SetTitle(KontorInformasjon[i].Kontor);
                Console.WriteLine("> Kontor ADDED: " + KontorInformasjon[i].Kontor + ";");
                offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));
                mMap.AddMarker(offices);
            
            }
            ValgteKontorer.Clear();
            
            Array.Clear(KontorInformasjon, 0, KontorInformasjon.Length);
        }
	


		public void OnInfoWindowClick (Marker marker)
		{
			var tlf = marker.Snippet;
			var uri = Android.Net.Uri.Parse("tel:"+ tlf);
			var intent = new Intent(Intent.ActionView, uri);
			StartActivity(intent);
		}

	
		public bool OnMyLocationButtonClick ()
		{
			LocationManager manager = (LocationManager) GetSystemService( Context.LocationService);

			try {
				Location lokasjon = mMap.MyLocation;
				mMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(lokasjon.Latitude, lokasjon.Longitude), 11));	

			} catch (Exception e) {

				if (!manager.IsProviderEnabled (LocationManager.GpsProvider)) {

					Toast.MakeText (this, "GPS må aktiveres", ToastLength.Long).Show ();

					StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));

				} else {
					Toast.MakeText (this, "Venter på GPS", ToastLength.Long).Show ();
				}
			}
			return true;
		}


        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Resource.Id.OfficeCheckList:

                    showSelection();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.actionbar_buttons_Maps, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        


        public void showSelection()
        {
            
            AlertDialog.Builder _builder = new AlertDialog.Builder(this);
            var inputView = LayoutInflater.Inflate(Resource.Layout.KontorValg, null);


            _builder.SetView(inputView);
            _builder.SetCancelable(true);
            _builder.SetPositiveButton("Ok", OkClicked);


            
            AlertDialog DialogView = _builder.Create();
            DialogView.Show();

            Kemnern = DialogView.FindViewById<Switch>(Resource.Id.map_Switch1);
            Skatteoppkrever = DialogView.FindViewById<Switch>(Resource.Id.map_Switch2);
            Kommunekasserer = DialogView.FindViewById<Switch>(Resource.Id.map_Switch3);
            Skattekontorer = DialogView.FindViewById<Switch>(Resource.Id.map_Switch4);
            Sentralskattekontoret = DialogView.FindViewById<Switch>(Resource.Id.map_Switch5);
            Oljeskattekontoret = DialogView.FindViewById<Switch>(Resource.Id.map_Switch6);
            Skattedirektoratet = DialogView.FindViewById<Switch>(Resource.Id.map_Switch7);
            Servicepartner = DialogView.FindViewById<Switch>(Resource.Id.map_Switch8);
            SITSBrukersenter = DialogView.FindViewById<Switch>(Resource.Id.map_Switch9);
            Registerinfo = DialogView.FindViewById<Switch>(Resource.Id.map_Switch10);

            if (((int)Android.OS.Build.VERSION.SdkInt) < 21)
            {
                //setSwitchColor();'
                Kemnern.SetThumbResource(Resource.Drawable.switch_thumb);
                Skatteoppkrever.SetThumbResource(Resource.Drawable.switch_thumb);
                Kommunekasserer.SetThumbResource(Resource.Drawable.switch_thumb);
                Skattekontorer.SetThumbResource(Resource.Drawable.switch_thumb);
                Sentralskattekontoret.SetThumbResource(Resource.Drawable.switch_thumb);
                Oljeskattekontoret.SetThumbResource(Resource.Drawable.switch_thumb);
                Skattedirektoratet.SetThumbResource(Resource.Drawable.switch_thumb);
                Servicepartner.SetThumbResource(Resource.Drawable.switch_thumb);
                SITSBrukersenter.SetThumbResource(Resource.Drawable.switch_thumb);
                Registerinfo.SetThumbResource(Resource.Drawable.switch_thumb);
            }


            if (ValgteSwitcher.Count != 0)
            {
                if (ValgteSwitcher.Contains("Kemnern"))
                {
                    Kemnern.Checked = true;
                }

                if (ValgteSwitcher.Contains("Skatteoppkrever"))
                { 
                    Skatteoppkrever.Checked = true; 
                }

                if (ValgteSwitcher.Contains("Kommunekasserer"))
                { Kommunekasserer.Checked = true; }

                if (ValgteSwitcher.Contains("Skattekontorer"))
                { Skattekontorer.Checked = true; }

                if (ValgteSwitcher.Contains("Sentralskattekontoret"))
                { Sentralskattekontoret.Checked = true; }

                if (ValgteSwitcher.Contains("Oljeskattekontoret"))
                { Oljeskattekontoret.Checked = true; }

                if (ValgteSwitcher.Contains("Skattedirektoratet"))
                { Skattedirektoratet.Checked = true; }

                if (ValgteSwitcher.Contains("Servicepartner"))
                { Servicepartner.Checked = true; }

                if (ValgteSwitcher.Contains("SITS Brukersenter"))
                { SITSBrukersenter.Checked = true; }

                if (ValgteSwitcher.Contains("Registerinfo"))
                { Registerinfo.Checked = true; }

            }





            ValgteSwitcher.Clear();
        }

        string[] KemnernL = { "Kemnern", "kemnerkontor" };
        string[] SkatteoppkreverL = { "skatteoppkreverkontor", "skatteoppkreverktr.", "Skatteoppkreveren", "Skatteoppkrevjaren", "skatteoppkrevjarkontor", "skatteoppkreverktr" };
        string[] KommunekassererL = { "kommunekassererkontor", "kommunekasserarkontor" };
        string[] SkattekontorerL = { "Skatt øst", "Skatt sør" , "Skatt vest", "Skatt Midt-Norge" , "Skatt nord", "Skatt Nord" };


        private void OkClicked(object sender, DialogClickEventArgs args)
        {
            var dialog = (AlertDialog)sender;



            Kemnern = dialog.FindViewById<Switch>(Resource.Id.map_Switch1);
            Skatteoppkrever = dialog.FindViewById<Switch>(Resource.Id.map_Switch2);
            Kommunekasserer = dialog.FindViewById<Switch>(Resource.Id.map_Switch3);
            Skattekontorer = dialog.FindViewById<Switch>(Resource.Id.map_Switch4);
            Sentralskattekontoret = dialog.FindViewById<Switch>(Resource.Id.map_Switch5);
            Oljeskattekontoret = dialog.FindViewById<Switch>(Resource.Id.map_Switch6);
            Skattedirektoratet = dialog.FindViewById<Switch>(Resource.Id.map_Switch7);
            Servicepartner = dialog.FindViewById<Switch>(Resource.Id.map_Switch8);
            SITSBrukersenter = dialog.FindViewById<Switch>(Resource.Id.map_Switch9);
            Registerinfo = dialog.FindViewById<Switch>(Resource.Id.map_Switch10);














            ValgteKontorer = new List<string>();
            ValgteSwitcher = new List<string>();

            if (Kemnern.Checked == true)
            {
                ValgteSwitcher.Add("Kemnern");
                ValgteKontorer.AddRange(KemnernL);
            }
            if (Skatteoppkrever.Checked == true)
            { ValgteSwitcher.Add("Skatteoppkrever"); ValgteKontorer.AddRange(SkatteoppkreverL); }
            if (Kommunekasserer.Checked == true)
            { ValgteSwitcher.Add("Kommunekasserer"); ValgteKontorer.AddRange(KommunekassererL); }
            if (Skattekontorer.Checked == true)
            { ValgteSwitcher.Add("Skattekontorer"); ValgteKontorer.AddRange(SkattekontorerL); }
            if (Sentralskattekontoret.Checked == true)
            { ValgteSwitcher.Add("Sentralskattekontoret"); ValgteKontorer.Add("Sentralskattekontoret"); }
            if (Oljeskattekontoret.Checked == true)
            { ValgteSwitcher.Add("Oljeskattekontoret"); ValgteKontorer.Add("Oljeskattekontoret"); }
            if (Skattedirektoratet.Checked == true)
            { ValgteSwitcher.Add("Skattedirektoratet"); ValgteKontorer.Add("Skattedirektoratet"); }
            if (Servicepartner.Checked == true)
            { ValgteSwitcher.Add("Servicepartner"); ValgteKontorer.Add("Skatteetatens IT- og servicepartner"); }
            if (SITSBrukersenter.Checked == true)
            { ValgteSwitcher.Add("SITS Brukersenter"); ValgteKontorer.Add("SITS Brukersenter"); }
            if (Registerinfo.Checked == true)
            { ValgteSwitcher.Add("Registerinfo"); ValgteKontorer.Add("Registerinfo"); }


            SetMarkers();
        }

	}
}

