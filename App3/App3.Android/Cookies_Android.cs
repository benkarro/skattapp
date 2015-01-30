using Android.Webkit;
using App3.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency (typeof (Cookies_Android))]

namespace App3.Droid
{
    class Cookies_Android: CookiesInterface
    {
        
        public Cookies_Android() { }

        public string GetCookie(String Url)
        {
            
            return CookieManager.Instance.GetCookie(Url);
        }


    }
}
