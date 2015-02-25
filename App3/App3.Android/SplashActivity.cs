namespace App3.Droid
{
	using System.Threading;

	using Android.App;
	using Android.OS;

	[Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			Thread.Sleep(1000); // Simulate a long loading process on app startup.
			StartActivity(typeof(MainActivity));
		}
	}
}

