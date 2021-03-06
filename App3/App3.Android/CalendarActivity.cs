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
using System.Net.Http;
using System.Xml.Linq;
using App3.Parser_xml;
using Android.Provider;
using System.Xml;
using Java.Util;

namespace App3.Droid
{
    [Activity(Label = "Skatte Kalender", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class CalendarActivity : Activity
    {
        private XMLroot[] _items;
        int CalendarChoice;
		bool ShowExpiredEvents = true;

		ListView CalendarList;
		calendarAdapter CalendarAdapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Xamarin.Forms.Forms.Init(this, bundle);

			SetContentView (Resource.Layout.Calendar);

			CalendarList = FindViewById<ListView> (Resource.Id.CalendarList);

            //
            
            

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            //
            getxmlStringandParse();

            retrieveset();

			RegisterForContextMenu(this.CalendarList);

			CalendarList.ItemClick += CalendarList_ItemClick;

        }

        void CalendarList_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {
			string title;
			string link;
			string subject;

			if (_items[e.Position].title != null) 
			{ title = _items[e.Position].title.ToString(); }

			if (_items[e.Position].link != null)  
			{ link = _items[e.Position].link.ToString();} else 
			{ return; }

			if (_items[e.Position].description != null) 
			{ subject = _items[e.Position].description.ToString(); }

			Android.Net.Uri uri = Android.Net.Uri.Parse(link);

			Intent intent = new Intent(Intent.ActionView);
			intent.SetData(uri);
			Intent chooser = Intent.CreateChooser(intent, "�pne linken med");

			this.StartActivity(chooser); 
        }


        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);

            MenuInflater mf = new MenuInflater(this);
            mf.Inflate(Resource.Menu.contextmenuCalendar, menu);
        }



        protected void retrieveset()
        {
            
        }



        public async void getxmlStringandParse()
        {
            var prefs = Application.Context.GetSharedPreferences("Skatteetaten.perferences", FileCreationMode.Private);
            string CalendarSettings = prefs.GetString("Selected Calendar int", "");
			string Datosettings = prefs.GetString ("DateEx", ""); 
			if (bool.TrueString == Datosettings) 
			{ ShowExpiredEvents = true; } else 
			{ ShowExpiredEvents = false; }


            int output;
            bool result = int.TryParse(CalendarSettings, out output);
            if (result)
            {
                CalendarChoice = output;
            }

            /*Xml = http.DownloadXML();
            XmlString = await Xml;
            XmlParser = new XMLparser(XmlString);*/
            using (var client = new HttpClient())
            {
                var xmlFeed = "";
                if (CalendarChoice == 0) 
				{ xmlFeed = SkatteetatenXML.Skattekalender_Personer; }
				else if (CalendarChoice == 1)
				{ xmlFeed = SkatteetatenXML.Skattekalender_Bedrifter; }
				else
				{ xmlFeed = SkatteetatenXML.Skattekalender_Personer; }

                 

				if (ShowExpiredEvents == true) {

					_items = await SkatteetatenXML.XMLmain(xmlFeed);

				} else {

					XMLroot[] _temp;


					_temp = await SkatteetatenXML.XMLmain(xmlFeed);


					#region Finding Not Expired
					List<XMLroot> futureEvents = new List<XMLroot> ();
					for (int i = 0; i < _temp.Count (); i++) {
						#region Date Handling
						DateTime contentdate = XmlConvert.ToDateTime (_temp [i].date);
						DateTime currentdate = DateTime.Now;
						int Day = currentdate.Day;
						int Month = currentdate.Month;
						int Year = currentdate.Year;
						#endregion


						//Console.WriteLine("********************************************************************************************************");


						if (contentdate.Year == Year) {
							if (contentdate.Month > Month) {
								//Add to not expired
								//Console.WriteLine("NOT EXPIRED: [i] = " + i + " ITEM: " + _temp[i].title + "/" + _temp[i].link + "/" + _temp[i].date);
								futureEvents.Add (new XMLroot { title = _temp [i].title, link = _temp [i].link, date = _temp [i].date });

							} else if (contentdate.Month == Month) {
								if (contentdate.Day > Day) {
									//Add to not expired
									//Console.WriteLine("NOT EXPIRED: [i] = " + i + " ITEM: " + _temp[i].title + "/" + _temp[i].link + "/" + _temp[i].date);
									futureEvents.Add (new XMLroot { title = _temp [i].title, link = _temp [i].link, date = _temp [i].date });
								} else if (contentdate.Day == Day) {
									//Add to not expired
									//Console.WriteLine("NOT EXPIRED: [i] = " + i + " ITEM: " + _temp[i].title + "/" +  _temp[i].link + "/" + _temp[i].date);
									futureEvents.Add (new XMLroot { title = _temp [i].title, link = _temp [i].link, date = _temp [i].date });
								}
							}

						}
						//Console.WriteLine("********************************************************************************************************");
					}
					#endregion
					_items = futureEvents.ToArray ();

				}







				CalendarAdapter = new calendarAdapter(this, _items);
				CalendarList.SetAdapter(CalendarAdapter);
                //ListAdapter = new calendarAdapter(this, _items);
                //ls = new rssAdapter(this, _items);
            }



        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            base.OnContextItemSelected(item);

            var info = (AdapterView.AdapterContextMenuInfo) item.MenuInfo;


            var selectedItem = _items[info.Position];

            switch (item.ItemId)
            {

                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.startWebbrowserItem:
                    {
                        string title;
                        string link = "";
                        string subject;


                        if (_items[info.Position].title != null)
                        { title = _items[info.Position].title.ToString(); }

                        if (_items[info.Position].link != null)
                        { link = _items[info.Position].link.ToString(); }

                        if (_items[info.Position].description != null)
                        { subject = _items[info.Position].description.ToString(); }

                        Android.Net.Uri uri = Android.Net.Uri.Parse(link);

                        Intent intent = new Intent(Intent.ActionView);
                        intent.SetData(uri);
                        Intent chooser = Intent.CreateChooser(intent, "�pne linken med");

                        this.StartActivity(chooser);
                        break;
                    }

                case Resource.Id.setEventItem:
                    {

                        DateTime th = XmlConvert.ToDateTime(_items[info.Position].date);

                        int Day = th.Day;
                        int Month = th.Month; // Added - 1 for java calendar has January as 0 and not 1
                        int Year = th.Year;

                        int Hour = th.Hour;
                        int Minute = th.Minute;




                        DateTime DateVerify = XmlConvert.ToDateTime(_items[info.Position].date);

                        string VerifyText = _items[info.Position].title;
                        string TitleString;

                        /*if (VerifyText.Substring(0, 11).Contains(DateVerify.Date.ToShortDateString()))
                        {
                            TitleString = _items[info.Position].title.Replace(DateVerify.Date.ToShortDateString(), "").TrimStart();
                        }
                        else if (VerifyText.Substring(0, 25).Contains(DateVerify.ToString())) //If text contains date + time format.
                        {
                            TitleString = _items[info.Position].title;
                        }
                        else
                        {
                            TitleString = _items[info.Position].title;
                        }
					*/



					string DateTimeCheck = VerifyText.Substring (0, 11);
					DateTime ParseResult;

					bool res = DateTime.TryParse(DateTimeCheck, out ParseResult);
					if (res) {
						TitleString = _items [info.Position].title.Replace (DateTimeCheck, "").TrimStart ();
					} else {
						TitleString = _items [info.Position].title;
					}



                        ContentValues eventValues = new ContentValues();

                        eventValues.Put(CalendarContract.Events.InterfaceConsts.CalendarId, 1);
                        eventValues.Put(CalendarContract.Events.InterfaceConsts.Title, TitleString);
                        eventValues.Put(CalendarContract.Events.InterfaceConsts.Description, "Denne p�minnelsen har blitt lagt til av deg gjennom Skatteetaten appen");
                        eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtstart, GetDateTimeMS(th.Year, th.Month, th.Day, th.Hour, th.Minute));
                        eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtend, GetDateTimeMS(th.Year, th.Month, th.Day, th.Hour, th.Minute));
                        /*eventValues.Put(CalendarContract.Events.InterfaceConsts.AllDay, "0");
                        eventValues.Put(CalendarContract.Events.InterfaceConsts.HasAlarm, "1");*/
                        eventValues.Put(CalendarContract.Events.InterfaceConsts.EventTimezone, "UTC"); // Set Timezon for appointment
                        eventValues.Put(CalendarContract.Events.InterfaceConsts.EventEndTimezone, "UTC");

                        var evnetUri = this.ContentResolver.Insert(CalendarContract.Events.ContentUri, eventValues);
                        long eventID = long.Parse(evnetUri.LastPathSegment);

                        ContentValues remindervalues = new ContentValues();
                        remindervalues.Put(CalendarContract.Reminders.InterfaceConsts.Minutes, 30);
                        remindervalues.Put(CalendarContract.Reminders.InterfaceConsts.EventId, eventID);
                        remindervalues.Put(CalendarContract.Reminders.InterfaceConsts.Method, (int)Android.Provider.RemindersMethod.Alert);
                        var ReminderURI = this.ContentResolver.Insert(CalendarContract.Reminders.ContentUri, remindervalues);




                        Console.WriteLine("Uri for new event: {0}", ReminderURI);

                        break;
                    }
            }





            return true; 
        }

        long GetDateTimeMS(int Year, int Month, int Day, int Hour, int Minute)
        {
            Calendar c = Calendar.GetInstance(Java.Util.TimeZone.Default);

            c.Set(Calendar.DayOfMonth, Day);
            c.Set(Calendar.HourOfDay, Hour);
            c.Set(Calendar.Minute, Minute);
            c.Set(Calendar.Month, Month -1);
            c.Set(Calendar.Year, Year);

            return c.TimeInMillis;
        }

    }
}