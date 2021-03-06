﻿using Android.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms;
using App3.Parser;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace App3
{
    public class HttpHelper2
    {

        
        public HttpHelper2(String Url)
        {
            this.Url = Url;
        }

        
		public String resultString { get; private set; }
        public Boolean finish { get; set; }
        String Url;

        
 
		public async Task<String> DownloadJson()
        {                  

            // Add a new Request Message
          
            HttpWebRequest requestMessage = (HttpWebRequest)WebRequest.Create(new Uri(Url));

            requestMessage.Method = WebRequestMethods.Http.Get;
            requestMessage.Headers.Add("Cookie", DependencyService.Get<CookiesInterface>().GetCookie(Url));

            requestMessage.Host="www.altinn.no";
            requestMessage.Accept= "application/hal+json";
            requestMessage.Headers.Add("ApiKey", Resources.Strings.ApiKey); //Api Key is under (Shared) Resources/Strings.cs 
			requestMessage.Headers.Add ("Accept-Charset", "utf-8");
           
            HttpWebResponse response = (HttpWebResponse)await requestMessage.GetResponseAsync();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                resultString=sr.ReadToEnd();
				Console.WriteLine (resultString);
				return  resultString;
            }
            
        }

        public async Task<String> DownloadXML()
        {
            HttpWebRequest requestDoc = (HttpWebRequest)WebRequest.Create(new Uri(Url));

            requestDoc.Method = WebRequestMethods.Http.Get;


            HttpWebResponse response = (HttpWebResponse)await requestDoc.GetResponseAsync();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                resultString = sr.ReadToEnd();
                Console.WriteLine(resultString);
                return resultString;
            }

        }
        

        
    }
}
