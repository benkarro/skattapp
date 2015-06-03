using System.Timers;

namespace App3.Droid
{
	using System.Threading;

	using Android.App;
	using Android.OS;

	[Activity(MainLauncher = true, NoHistory = true,
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize,
        Theme = "@android:style/Theme.Holo.Light.NoActionBar.Fullscreen")]
	public class SplashActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			//base.OnCreate(savedInstanceState);
			//Thread.Sleep(1000); // Simulate a long loading process on app startup.
			//StartActivity(typeof(MainActivity));
			//ActionBar.Hide ();
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Splash);




			System.Timers.Timer timer = new System.Timers.Timer();
			timer.Interval = 3000; // 3 sec.
			timer.AutoReset = false; // Do not reset the timer after it's elapsed
			timer.Elapsed += (object sender, ElapsedEventArgs e) =>
			{
				StartActivity(typeof(MainActivity));
				timer.Stop();
				timer.Dispose();
				Finish();
			};
			timer.Start();
		}
			

	}


}

