using Android.App;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Views;
using Android.Content;

namespace App3.Droid
{
	[Activity ()]			
	public class MapsActivity : Activity, IOnMapReadyCallback, GoogleMap.IInfoWindowAdapter, GoogleMap.IOnInfoWindowClickListener
	{

		private GoogleMap mMap;

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
			LatLng posission = new LatLng (40.776408, -73.970755);
			LatLng posission2 = new LatLng (40, -73);


			CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom (posission, 10);
			mMap.MoveCamera (camera);


			mMap.SetInfoWindowAdapter (this);

			MarkerOptions options = new MarkerOptions ()
				.SetPosition (posission)
				.SetTitle("New york")
				.SetSnippet("aka big town");
			mMap.AddMarker (options);



			mMap.AddMarker (new MarkerOptions ()
				.SetPosition (posission2)
				.SetTitle ("yo")
				.SetSnippet("aka big town")
			);

		
			mMap.SetOnInfoWindowClickListener (this);





		}

		public Android.Views.View GetInfoContents (Marker marker)
		{
			return null;
		}

		public Android.Views.View GetInfoWindow (Marker marker)
		{
			View view = LayoutInflater.Inflate (Resource.Layout.MapPoint, null);
			return view;
		}


		public void OnInfoWindowClick (Marker marker)
		{
			var uri = Android.Net.Uri.Parse("tel:test");
			var intent = new Intent(Intent.ActionView, uri);
			StartActivity(intent);
		}
	}
}

