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
using Android.Graphics.Drawables;

namespace App3.Droid
{
    [Activity(Label = "Innstillinger", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class SettingsActivity : Activity
    {

        Switch SMSswitch;
        Spinner RSSselector;

        List<string> _thisItems = new List<string>();
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.Settings);


            SMSswitch = FindViewById<Switch>(Resource.Id.switch1);
            RSSselector = FindViewById<Spinner>(Resource.Id.spinner1);


            //IF Android version is lower than Lollipop
            if (((int)Android.OS.Build.VERSION.SdkInt) < 21)
            {
                //setSwitchColor();'

                SMSswitch.SetThumbResource(Resource.Drawable.switch_thumb);
            }



            SMSswitch.CheckedChange += delegate(object sender, CompoundButton.CheckedChangeEventArgs e)
            {
                var toast = Toast.MakeText(this, "SMS reader is " + SMSswitch.Checked.ToString() + "/" + (e.IsChecked.ToString()), ToastLength.Short);
                toast.Show();
            };

            _thisItems.Add("For personer");
            _thisItems.Add("For bedrifter og organisasjoner"); 


            RSSselector.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);


            var adapter = new ArrayAdapter(this,

            Android.Resource.Layout.SimpleSpinnerItem, _thisItems);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);


            RSSselector.Adapter = adapter;





            retrieveset();
            //setSwitchColor();


        }

        /// <summary>
        /// string cvCo = Application.Context.GetString(Resource.Color.Secondary5);
                               /// Android.Graphics.Color thCol = new Android.Graphics.Color(Android.Graphics.Color.ParseColor(cvCo));
                                ///rl.Background = new ColorDrawable(thCol);
        /// </summary>
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            /*string toast = string.Format("The planet is {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();*/
        }


        protected override void OnDestroy()
        {
            saveset();
            base.OnDestroy();
        }

        protected void saveset()
        {
            Switch SMSswitch = FindViewById<Switch>(Resource.Id.switch1);
            Spinner Calendar = FindViewById<Spinner>(Resource.Id.spinner1);

            var prefs = Application.Context.GetSharedPreferences("Skatteetaten.perferences", FileCreationMode.Private);
            var prefEdityor = prefs.Edit();
            prefEdityor.PutString("ReadSMS", SMSswitch.Checked.ToString());
            prefEdityor.PutString("Selected Calendar int", Calendar.SelectedItemPosition.ToString());
            prefEdityor.PutString("Selected Calendar string", Calendar.SelectedItem.ToString());
            prefEdityor.Commit();

        }

        protected void retrieveset()
        {
            var prefs = Application.Context.GetSharedPreferences("Skatteetaten.perferences", FileCreationMode.Private);
            var SMSsettings = prefs.GetString("ReadSMS", "");
            string CalendarSettings = prefs.GetString("Selected Calendar int", "0");

            if (bool.TrueString == SMSsettings) 
            { SMSswitch.Checked = true; } else 
            { SMSswitch.Checked = false; }



            RSSselector.SetSelection(int.Parse(CalendarSettings));
            

           /* RunOnUiThread(() => {
                Toast.MakeText(this, SMSsettings, ToastLength.Short).Show();
            });*/
        }

        protected override void OnPause()
        {
            base.OnPause();
            saveset();
        }

    }

}