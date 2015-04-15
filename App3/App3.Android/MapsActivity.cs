using Android.App;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace App3.Droid
{
	[Activity ()]			
	public class MapsActivity : Activity
	{
		
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// add map fragment to frame layout
			MapFragment mapFragment = (MapFragment) FragmentManager.FindFragmentById(Resource.Id.map);
			mapFragment = MapFragment.NewInstance();
		//	FragmentTransaction fragmentTx = this.FragmentManager.BeginTransaction ();
		//	fragmentTx.Add (Resource.Id.map, mapFragment);
		//	fragmentTx.Commit ();


			GoogleMap map = mapFragment.Map;

			MapsInitializer.Initialize(Application.Context);
			LatLng location = new LatLng(50.897778, 3.013333);
			CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
			builder.Target(location);
			builder.Zoom(18);
			builder.Bearing(155);
			builder.Tilt(65);

			CameraPosition cameraPosition = builder.Build();
			CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
			if (map != null) {
				



				map.MoveCamera(cameraUpdate);			
			}

			// Set our view from the "Maps" layout resource
			SetContentView (Resource.Layout.Maps);
		}


	}
}

