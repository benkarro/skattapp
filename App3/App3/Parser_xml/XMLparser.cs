using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;

namespace App3.Parser_xml
{
    class XMLparser
    {

        public List<XMLroot> items;




        public XMLparser()
        {
            //string XML = RemoveAllNamespaces(oXML);
            Console.WriteLine("¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤");
            //Console.WriteLine(XML);
            Console.WriteLine("¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤");

            //XDocument doc = XDocument.Parse(XML)
          


               /* XDocument doc = XDocument.Parse(XML);
                
                var elements = doc.Descendants("item").Select(x => new XMLroot
                {
                    title = x.Element("title").Value,
                    description = x.Element("description").Value,
                    link = x.Element("link").Value,
                    date = x.Element("date").Value

                }).ToList();*/



#region Tried
                /*var sz = new XmlSerializer(typeof(XMLroot), new XmlRootAttribute("rss")); //rss or channel

                using (var stringReader = new StringReader(XML))
                using (var reader = XmlReader.Create(stringReader)) 
                {
                    var result = (XMLroot)sz.Deserialize(reader);
                    Console.WriteLine(result.title[1]);
                }*/


                /*XmlSerializer serializer = new XmlSerializer(typeof, "");

                TextReader reader = new StringReader(XML);

                XMLroot root;

                root = (XMLroot)serializer.Deserialize(reader);

                Console.Write(
                    root.title + "\t" +
                    root.link + "\t" +
                    root.description + "\t" +
                    root.date + "\t"                    
                    );*/



                /*var rssFeed = XDocument.Load(XML);

                var posts = from item in rssFeed.Descendants("item")
                            select new XMLroot
                            {
                                title = item.Element("title").Value,
                                description = item.Element("description").Value,
                                date = item.Element("pubDate").Value
                            };*/

            /*items = (from x in doc.Root.Elements("item")
                     select new XMLroot
                     {
                         title = (string)x.Element("title").Value,
                         link = (string)x.Element("link").Value,
                         description = (string)x.Element("description").Value,
                         date = (string)x.Element("dc:date").Value
                     }).ToList();*/

           
               /*items = (from x in doc.Descendants("item")
                         select new XMLroot()
                         {
                             title = x.Element("title").Value,
                             link = x.Element("link").Value,
                             description = x.Element("description").Value,
                             date = x.Element("dc:date").Value
                         }).ToList();*/



                /*var serializer = new XmlSerializer(typeof(XMLroot));

                using (TextReader reader = new StringReader(XML))
                {
                    result = (StatusDocumentItem)serializer.Deserialize(reader);
                }

                Console.WriteLine(result.Message);*/
#endregion

        }


        /*public static IEnumerable<XMLroot> getChannelquery(XDocument doc)
        {
           return from channels in doc.Descendants("channel")
                   select new XMLroot
                   {
                       title = channels.Element("title") != null ? channels.Element("title").Value : "",
                       link = channels.Element("link") != null ? channels.Element("link").Value : "",
                       description = channels.Element("description") != null ? channels.Element("description").Value : "",

                       Rootitems = from items in channels.Descendants("item")
                                   select new Element
                                       {
                                           title = items.Element("title").Value != null ? items.Element("title").Value : "",
                                           link = items.Element("link") != null ? items.Element("link").Value : "",
                                           date = items.Element("date") != null ? items.Element("date").Value : "",
                                           description = items.Element("description") != null ? items.Element("description").Value : ""
                                       }

                   };
        }*/


















        // helper class to ignore namespaces when de-serializing

        //Implemented based on interface, not part of algorithm
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

    }
}
