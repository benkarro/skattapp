using System;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using App3.Parser_xml;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App3
{
	public class SkatteetatenXML
	{
		//private XMLroot[] _items;
		public static List<XMLroot> XmlList = new List<XMLroot>();
		public static string XmlNamespace = "http://purl.org/dc/elements/1.1/";
		public static string Pressemeldinger_HTTP = "http://www.skatteetaten.no/no/Stottemenyer/RSS-feeder/Pressemeldinger/";

		public static string Skattekalender_Personer = "http://www.skatteetaten.no/no/Stottemenyer/RSS-feeder/Skattekalender-for-personer/";
		public static string Skattekalender_Bedrifter = "http://www.skatteetaten.no/no/Stottemenyer/RSS-feeder/Kalender-for-bedrifter-og-organisasjoner/";


		public SkatteetatenXML ()
		{
		}

		public async static Task<XMLroot[]> XMLmain(string Url) 
		{
			XMLroot[] _items;
			//XmlList = new List<XMLroot> ();
			using (var XmlClient = new HttpClient()) 
			{
				var XmlString = await Fetcher (XmlClient, Url);

				var Document = XDocument.Parse (XmlString);
				XNamespace Namespace = XmlNamespace;

				_items = XmlrootParser (Namespace, Document);

			}

			return _items;
		}


		public static async Task<string> Fetcher(HttpClient xClient, string Url) 
		{
			var XmlFeed = await xClient.GetStringAsync (Url);
			return XmlFeed.ToString();
		}


		public static XMLroot[] XmlrootParser(XNamespace Namespace, XDocument Document) 
		{
			XMLroot[] _items;

			_items = (from item in Document.Descendants("item")
				select new XMLroot
				{
					title = item.Element("title").Value,
					link = item.Element("link").Value,
					description = item.Element("description").Value,
					date = item.Element(Namespace + "date").Value
				}).ToArray();

			return _items;

		}
	}
}

