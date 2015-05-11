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


namespace App3.Droid
{
	[Activity ()]			
	public class MapsActivity : Activity, IOnMapReadyCallback, GoogleMap.IOnInfoWindowClickListener, GoogleMap.IOnMyLocationButtonClickListener
	
	{

		private GoogleMap mMap;

		private  List<String> kontorer;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "Maps" layout resource
			SetContentView (Resource.Layout.Maps);
			SetUpMap ();

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
	}
}

