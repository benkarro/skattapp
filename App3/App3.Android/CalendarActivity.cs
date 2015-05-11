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
    public class CalendarActivity : ListActivity
    {
        private XMLroot[] _items;
        int CalendarChoice;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Xamarin.Forms.Forms.Init(this, bundle);


            ActionBar.SetDisplayHomeAsUpEnabled(true);

            //
            getxmlStringandParse();

            retrieveset();

            RegisterForContextMenu(this.ListView);

        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            string title;
            string link;
            string subject;

            if (_items[position].title != null) 
            { title = _items[position].title.ToString(); }

            if (_items[position].link != null)  
            { link = _items[position].link.ToString();} else 
            { return; }

            if (_items[position].description != null) 
            { subject = _items[position].description.ToString(); }
            
            Android.Net.Uri uri = Android.Net.Uri.Parse(link);

            Intent intent = new Intent(Intent.ActionView);
            intent.SetData(uri);
            Intent chooser = Intent.CreateChooser(intent, "Åpne linken med");

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
                { 
                    xmlFeed= await client.GetStringAsync("http://www.skatteetaten.no/no/Stottemenyer/RSS-feeder/Skattekalender-for-personer/");
                }
                else if (CalendarChoice == 1)
                {
                    xmlFeed = await client.GetStringAsync("http://www.skatteetaten.no/no/Stottemenyer/RSS-feeder/Kalender-for-bedrifter-og-organisasjoner/");
                }
                else
                {
                    xmlFeed = await client.GetStringAsync("http://www.skatteetaten.no/no/Stottemenyer/RSS-feeder/Skattekalender-for-personer/");
                }

                 
                //var xmlFeed = await client.GetStringAsync("http://www.skatteetaten.no/no/Stottemenyer/RSS-feeder/Kalender-for-bedrifter-og-organisasjoner/");
                //

                var DOC = XDocument.Parse(xmlFeed);
                XNamespace dc = "http://purl.org/dc/elements/1.1/";

                _items = (from item in DOC.Descendants("item")
                          select new XMLroot
                          {
                              title = item.Element("title").Value,
                              link = item.Element("link").Value,
                              date = item.Element(dc + "date").Value
                          }).ToArray();
                //_items.Reverse();
                Array.Reverse(_items);

                ListAdapter = new calendarAdapter(this, _items);
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
                        Intent chooser = Intent.CreateChooser(intent, "Åpne linken med");

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

                        if (VerifyText.Substring(0, 11).Contains(DateVerify.Date.ToShortDateString()))
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




                        ContentValues eventValues = new ContentValues();

                        eventValues.Put(CalendarContract.Events.InterfaceConsts.CalendarId, 1);
                        eventValues.Put(CalendarContract.Events.InterfaceConsts.Title, TitleString);
                        eventValues.Put(CalendarContract.Events.InterfaceConsts.Description, "Denne påminnelsen har blitt lagt til av deg gjennom Skatteetaten appen");
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