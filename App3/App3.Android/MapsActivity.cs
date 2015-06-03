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
using System.Threading.Tasks;
using System.Threading;
using System.Linq;


namespace App3.Droid
{
	[Activity (ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]			
	public class MapsActivity : Activity, IOnMapReadyCallback, GoogleMap.IOnInfoWindowClickListener, GoogleMap.IOnMyLocationButtonClickListener
	{

		private GoogleMap mMap;

        public static KontorInfo[] KontorInformasjon;

        public static List<string> ValgteKontorer = new List<string>();
        public static List<string> ValgteSwitcher = new List<string>();

		public static Boolean KunSkatt = false;


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

		Switch Skattekontorer_Nord;
		Switch Skattekontorer_Midt;
		Switch Skattekontorer_Øst;
		Switch Skattekontorer_Vest;
		Switch Skattekontorer_Sør;
		LinearLayout map_Switch_KontorHolder;


		public String Oljeskattekontoret_color = Application.Context.GetString (Resource.Color.Primary1);
		public String Oljeskattekontoret_color_id = Resource.Style.sPrimary1.ToString();
		public String Skatteoppkrever_color = Application.Context.GetString (Resource.Color.Primary5); 
		public String Skatteoppkrever_color_id = Resource.Style.sPrimary5.ToString();
		public String Skattekontorer_color = Application.Context.GetString (Resource.Color.Primary4); 
		public String Skattekontorer_color_id = Resource.Style.sPrimary4.ToString();





		public String Skattedirektoratet_color = Application.Context.GetString (Resource.Color.Secondary1); 
		public String Skattedirektoratet_color_id = Resource.Style.sSecondary1.ToString();
		public String Sentralskattekontoret_color = Application.Context.GetString (Resource.Color.Secondary2); 
		public String Sentralskattekontoret_color_id = Resource.Style.sSecondary2.ToString();
		public String Kemnern_color = Application.Context.GetString (Resource.Color.Secondary3); 
		public String Kemnern_color_id = Resource.Style.sSecondary3.ToString();

		#region Secondary4
		public String Servicepartner_color = Application.Context.GetString (Resource.Color.Secondary4); 
		public String Servicepartner_color_id = Resource.Style.sSecondary4.ToString();
		public String SITSBrukersenter_color = Application.Context.GetString(Resource.Color.Secondary4); 
		public String SITSBrukersenter_color_id = Resource.Style.sSecondary4.ToString();
		public String Registerinfo_color = Application.Context.GetString (Resource.Color.Secondary4); 
		public String Registerinfo_color_id =  Resource.Style.sSecondary4.ToString();
		#endregion
		public String Kommunekasserer_color = Application.Context.GetString (Resource.Color.Secondary5); 
		public String Kommunekasserer_color_id =  Resource.Style.sSecondary5.ToString();


		public String Skattekontor_Nord_color = Application.Context.GetString(Resource.Color.Secondary5); 
		public String Skattekontor_Nord_color_id =  Resource.Style.sSecondary5.ToString();
		public String Skattekontor_Midt_color = Application.Context.GetString (Resource.Color.Primary1); 
		public String Skattekontor_Midt_color_id =  Resource.Style.sPrimary1.ToString();
		public String Skattekontor_Vest_color = Application.Context.GetString (Resource.Color.Primary3); 
		public String Skattekontor_Vest_color_id =  Resource.Style.sPrimary3.ToString();
		public String Skattekontor_Sør_color = Application.Context.GetString (Resource.Color.Secondary2); 
		public String Skattekontor_Sør_color_id =  Resource.Style.sSecondary2.ToString();
		public String Skattekontor_Øst_color = Application.Context.GetString (Resource.Color.Primary4); 
		public String Skattekontor_Øst_color_id =  Resource.Style.sPrimary4.ToString();



		//private  List<String> kontorer;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "Maps" layout resource
			SetContentView (Resource.Layout.Maps);


			GetSettings ();
            
            
            SetUpMap ();

		}


		public void GetSettings() 
		{


			var prefs = Application.Context.GetSharedPreferences("Skatteetaten.perferences", FileCreationMode.Private);

			var KunSkattsettings = prefs.GetString ("KunSkatt", "");

			if (bool.TrueString == KunSkattsettings) 
			{ KunSkatt = true; } else 
			{ KunSkatt = false; }


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


            //SetMarkers();
		}
			
		public async void SetMarkers()
        {
            mMap.Clear();
        
            KontorInformasjon = Kontorer.getKontorer(ValgteKontorer).ToArray();
            //Console.WriteLine(KontorInformasjon.Length);

			await Task.Run (() => {


				for (int i = 0; i < KontorInformasjon.Length; i++)
				{
					MarkerOptions offices = new MarkerOptions();
					offices.SetPosition(new LatLng(KontorInformasjon[i].Latitude, KontorInformasjon[i].Longitude));
					offices.SetTitle(KontorInformasjon[i].Kontor);
					Console.WriteLine("> Kontor ADDED: " + KontorInformasjon[i].Kontor + ";");



					string KontorTittel = KontorInformasjon[i].Kontor;


					string selectedColor = "";

					Android.Graphics.Color ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Skattekontorer_color));

					if (KontorTittel.Contains("Skattedirektoratet"))
					{
						selectedColor = Skattedirektoratet_color_id;

						ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Skattedirektoratet_color));
						var hue = ths.GetHue ();
						offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
					}
					else if (KontorTittel.Contains("Skatteetatens IT- og servicepartner"))
					{
						selectedColor = Servicepartner_color_id;

						ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Servicepartner_color));
						var hue = ths.GetHue ();
						offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
					}
					else if (KontorTittel.Contains("SITS Brukersenter"))
					{
						selectedColor = SITSBrukersenter_color_id;


						ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(SITSBrukersenter_color));
						var hue = ths.GetHue ();
						offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
					}
					else if (KontorTittel.Contains("Oljeskattekontoret"))
					{
						selectedColor = Oljeskattekontoret_color_id;


						ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Oljeskattekontoret_color));

						var hue = ths.GetHue ();
						offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
					}
					else if (KontorTittel.Contains("Sentralskattekontoret"))
					{
						selectedColor = Sentralskattekontoret_color_id;


						ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Sentralskattekontoret_color));

						var hue = ths.GetHue ();
						offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
					}
					else if (KontorTittel.Contains("Registerinfo"))
					{
						selectedColor = Registerinfo_color_id;


						ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Registerinfo_color));
						var hue = ths.GetHue ();
						offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
					}


					if (KontorTittel.Contains("Kemnern") || KontorTittel.Contains("kemnerkontor"))
					{
						selectedColor = Kemnern_color_id;


						ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Kemnern_color));

						var hue = ths.GetHue ();
						offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
					}
					else if (KontorTittel.Contains("kommunekassererkontor") || KontorTittel.Contains("kommunekasserarkontor"))
					{
						selectedColor = Kommunekasserer_color_id;

						ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Kommunekasserer_color));
						var hue = ths.GetHue ();
						offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
					}
					else if (KontorTittel.Contains("skatteoppkreverkontor") || KontorTittel.Contains("skatteoppkreverktr.") || KontorTittel.Contains("Skatteoppkreveren") || KontorTittel.Contains("Skatteoppkrevjaren") || KontorTittel.Contains("skatteoppkrevjarkontor") || KontorTittel.Contains("skatteoppkreverktr"))
					{
						selectedColor = Skatteoppkrever_color_id;

						ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Skatteoppkrever_color));
						var hue = ths.GetHue ();
						offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
					}

					if (ValgteSwitcher.Contains("Skattekontorer")) 
					{

						List<String> Verify = new List<string>();
						Verify.AddRange(SkattekontorerL);
						Verify.Add("Skattekontorer");

						List<String> Filter = new List<String>();


                        Filter.AddRange(ValgteKontorer.Where(x => !Verify.Contains(x)));




						if (Filter.Count == 0) {
							if (KontorTittel.Contains ("Skatt øst") || KontorTittel.Contains ("Skatt Øst")) 
							{

								selectedColor = Skattekontor_Øst_color_id;

								ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Skattekontor_Øst_color));
								var hue = ths.GetHue ();
								offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
							} 
							else if (KontorTittel.Contains ("Skatt vest") || KontorTittel.Contains ("Skatt Vest")) 
							{
								selectedColor = Skattekontor_Vest_color_id;

								ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Skattekontor_Vest_color));

								var hue = ths.GetHue ();
								offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
							} 
							else if (KontorTittel.Contains ("Skatt sør") || KontorTittel.Contains ("Skatt Sør")) 
							{
								selectedColor = Skattekontor_Sør_color_id;

								ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Skattekontor_Sør_color));

								var hue = ths.GetHue ();
								offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
							} 
							else if (KontorTittel.Contains ("Skatt Midt-Norge")) 
							{
								selectedColor = Skattekontor_Midt_color_id;

								ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Skattekontor_Midt_color));

								var hue = ths.GetHue ();
								offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
							} 
							else if (KontorTittel.Contains ("Skatt nord") || KontorTittel.Contains ("Skatt Nord")) 
							{
								selectedColor = Skattekontor_Nord_color_id;


								ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Skattekontor_Nord_color));

								var hue = ths.GetHue ();
								offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
							}
						} 
						else if (KontorTittel.Contains ("Skatt øst") || KontorTittel.Contains ("Skatt Øst") || 
							KontorTittel.Contains ("Skatt vest") || KontorTittel.Contains ("Skatt Vest") || 
							KontorTittel.Contains ("Skatt sør") || KontorTittel.Contains ("Skatt Sør") || 
							KontorTittel.Contains ("Skatt nord") || KontorTittel.Contains ("Skatt Nord") ||
							KontorTittel.Contains ("Skatt Midt-Norge") 
						)
						{
							selectedColor = Skattekontorer_color_id;


							ths = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(Skattekontorer_color));

							var hue = ths.GetHue ();
							offices.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(hue));
						}
					}







					offices.SetSnippet (i.ToString() + ";" + selectedColor);

					RunOnUiThread(() => {
						mMap.AddMarker(offices);
					});
				}
				RunOnUiThread(() => {
				mMap.SetInfoWindowAdapter(new CustomMarkerPopupAdapter(LayoutInflater));
				});

			});


            ValgteKontorer.Clear();
            
            
        }
	


		public void OnInfoWindowClick (Marker marker)
		{
			Intent intent = new Intent(this, typeof(KontorActivity));


			int nint;

			string s = marker.Snippet.ToString ();

			string[] splitchar = { ";" };
			string[] split = s.Split(splitchar, StringSplitOptions.RemoveEmptyEntries);

			if (split.Length == 2) 
			{
				bool res = (int.TryParse (split[0].ToString(), out nint));
				if (res) {
					if (KontorInformasjon.Length >= nint) { 

						intent.PutExtra ("Kontor", KontorInformasjon [nint].Kontor);
						intent.PutExtra ("Åpningstid", KontorInformasjon [nint].Åpent);
						intent.PutExtra ("Telefon", KontorInformasjon [nint].Nummer1.ToString());
                        intent.PutExtra("Telefaks", KontorInformasjon[nint].Nummer2.ToString());
						intent.PutExtra ("Adresse", KontorInformasjon [nint].Addresse);
						intent.PutExtra ("Postboks", KontorInformasjon [nint].Postboks);
						intent.PutExtra ("Postkode1", KontorInformasjon [nint].Postkode1);
						intent.PutExtra ("Postkode2", KontorInformasjon [nint].PostKode2);
						intent.PutExtra ("Epost", KontorInformasjon [nint].Epost);

                        intent.PutExtra("Latitude", KontorInformasjon[nint].Latitude.ToString());
                        intent.PutExtra("Longitude", KontorInformasjon[nint].Longitude.ToString());


					}
				}

				intent.PutExtra ("StyleID", split [1]);

			} 











			Console.WriteLine (marker.Id);






			//var tlf = marker.Snippet;
			//var uri = Android.Net.Uri.Parse("tel:"+ tlf);
			//var intent = new Intent(Intent.ActionView, uri);
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
			Skattekontorer = DialogView.FindViewById<Switch> (Resource.Id.map_Switch4);

			Skattekontorer_Nord = DialogView.FindViewById<Switch> (Resource.Id.map_Switch4_1);
			Skattekontorer_Midt = DialogView.FindViewById<Switch> (Resource.Id.map_Switch4_2);
			Skattekontorer_Øst = DialogView.FindViewById<Switch> (Resource.Id.map_Switch4_3);
			Skattekontorer_Vest = DialogView.FindViewById<Switch> (Resource.Id.map_Switch4_4);
			Skattekontorer_Sør = DialogView.FindViewById<Switch> (Resource.Id.map_Switch4_5);

			map_Switch_KontorHolder = DialogView.FindViewById<LinearLayout> (Resource.Id.map_Switch_KontorHolder);


			Skattekontorer.CheckedChange += (sender, e) => {
				
				if (Skattekontorer.Checked == true) {
					map_Switch_KontorHolder.Visibility = ViewStates.Visible;

				} 
				else 
				{
					map_Switch_KontorHolder.Visibility = ViewStates.Gone;
				}
			};

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


			if (KunSkatt == true) 
			{
				Kemnern.Visibility = ViewStates.Gone;
				Kommunekasserer.Visibility = ViewStates.Gone;
				Registerinfo.Visibility = ViewStates.Gone;
			}





			map_Switch_KontorHolder = DialogView.FindViewById<LinearLayout> (Resource.Id.map_Switch_KontorHolder);

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

				Skattekontorer_Nord.SetThumbResource(Resource.Drawable.switch_thumb);
				Skattekontorer_Midt.SetThumbResource(Resource.Drawable.switch_thumb);
				Skattekontorer_Øst.SetThumbResource(Resource.Drawable.switch_thumb);
				Skattekontorer_Vest.SetThumbResource(Resource.Drawable.switch_thumb);
				Skattekontorer_Sør.SetThumbResource(Resource.Drawable.switch_thumb);
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
                { 
					Skattekontorer.Checked = true; 

					if (ValgteSwitcher.Count == 1) 
					{
						map_Switch_KontorHolder.Visibility = ViewStates.Visible;
					}

					if (ValgteSwitcher.Contains ("Skattekontorer_Nord")) {Skattekontorer_Nord.Checked = true;}
					if (ValgteSwitcher.Contains ("Skattekontorer_Midt")) {Skattekontorer_Midt.Checked = true;}
					if (ValgteSwitcher.Contains ("Skattekontorer_Øst")) {Skattekontorer_Øst.Checked = true;}
					if (ValgteSwitcher.Contains ("Skattekontorer_Vest")) {Skattekontorer_Vest.Checked = true;}
					if (ValgteSwitcher.Contains ("Skattekontorer_Sør")) {Skattekontorer_Sør.Checked = true;}

				}

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
		string[] SkattekontorerL = { "Skatt nord", "Skatt Nord" , "Skatt Midt-Norge", "Skatt midt-norge" , "Skatt Vest", "Skatt vest" , "Skatt Øst", "Skatt øst" , "Skatt Sør", "Skatt sør" };
		string[] Skattekontor_Nord = {"Skatt nord", "Skatt Nord"};
		string [] Skattekontor_Midt = {"Skatt Midt-Norge", "Skatt midt-norge"};
		string [] Skattekontor_Vest = {"Skatt Vest", "Skatt vest"};
		string [] Skattekontor_Øst = {"Skatt Øst", "Skatt øst"};
		string [] Skattekontor_Sør = {"Skatt Sør", "Skatt sør"};


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



			Skattekontorer_Nord = dialog.FindViewById<Switch> (Resource.Id.map_Switch4_1);
			Skattekontorer_Midt = dialog.FindViewById<Switch> (Resource.Id.map_Switch4_2);
			Skattekontorer_Øst = dialog.FindViewById<Switch> (Resource.Id.map_Switch4_3);
			Skattekontorer_Vest = dialog.FindViewById<Switch> (Resource.Id.map_Switch4_4);
			Skattekontorer_Sør = dialog.FindViewById<Switch> (Resource.Id.map_Switch4_5);

			map_Switch_KontorHolder = dialog.FindViewById<LinearLayout> (Resource.Id.map_Switch_KontorHolder);








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
            { 
				ValgteSwitcher.Add("Skattekontorer"); 

				if (map_Switch_KontorHolder.Visibility == ViewStates.Visible) 
				{
					if (Skattekontorer_Nord.Checked == true) { ValgteSwitcher.Add ("Skattekontorer_Nord"); ValgteKontorer.AddRange (Skattekontor_Nord); }
					if (Skattekontorer_Midt.Checked == true) { ValgteSwitcher.Add ("Skattekontorer_Midt"); ValgteKontorer.AddRange (Skattekontor_Midt); }
					if (Skattekontorer_Vest.Checked == true) { ValgteSwitcher.Add ("Skattekontorer_Vest"); ValgteKontorer.AddRange (Skattekontor_Vest); }
					if (Skattekontorer_Øst.Checked == true) { ValgteSwitcher.Add ("Skattekontorer_Øst"); ValgteKontorer.AddRange (Skattekontor_Øst); }
					if (Skattekontorer_Sør.Checked == true) { ValgteSwitcher.Add ("Skattekontorer_Sør"); ValgteKontorer.AddRange ( Skattekontor_Sør); }
				} 
				else 
				{
					ValgteKontorer.AddRange(SkattekontorerL);
				}

				 
			}
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






		public class CustomMarkerPopupAdapter : Java.Lang.Object, GoogleMap.IInfoWindowAdapter
		{
			private LayoutInflater _layoutInflater = null;

			public CustomMarkerPopupAdapter(LayoutInflater inflater)
			{
				_layoutInflater = inflater;
			}

			public View GetInfoWindow(Marker marker)
			{
				return null;
			}

			public View GetInfoContents(Marker marker)
			{
				
				var customPopup = _layoutInflater.Inflate(Resource.Layout.CustomMarkerWindow, null);

				var titleTextView = customPopup.FindViewById<TextView>(Resource.Id.CMW_textView1);
				if (titleTextView != null)
				{
					titleTextView.Text = marker.Title;
				}







				int nint;

				string s = marker.Snippet.ToString ();

				string[] splitchar = { ";" };
				string[] split = s.Split(splitchar, StringSplitOptions.RemoveEmptyEntries);

				if (split.Length == 2) 
				{
					bool res = (int.TryParse (split[0].ToString(), out nint));
					if (res) {
						if (KontorInformasjon.Length >= nint) { 

							string Åpent = KontorInformasjon[nint].Åpent;
							int Tlf = KontorInformasjon[nint].Nummer1;
							string Adr = KontorInformasjon[nint].Addresse;

							TextView Åpningsitd = customPopup.FindViewById<TextView>(Resource.Id.CMW_time_textView2);
							TextView Telefon = customPopup.FindViewById<TextView>(Resource.Id.CMW_telefon_textView2);
							TextView Adresse = customPopup.FindViewById<TextView>(Resource.Id.CMW_adresse_textView2);

							Åpningsitd.Text = Åpent;
							Telefon.Text = Tlf.ToString();
							Adresse.Text = Adr;


						}
					}
						
				} 

				return customPopup;
			}
		}



		protected override void OnDestroy ()
		{
			base.OnDestroy ();

			//this.Dispose ();

			
			Finish ();

            GC.Collect(GC.MaxGeneration);

		}


	}
}

