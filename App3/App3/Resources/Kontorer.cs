using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace App3.Resources
{
    public class Kontorer
    {

        public static List<KontorInfo> Kontorinf = new List<KontorInfo>();



        public static List<Holding> indexes = new List<Holding>();
        public static Holding[] index;


        public static Holding hld;




        public static List<KontorInfo> getKontorer(List<string> Vis)
        {
            indexes = new List<Holding>();
            Kontorinf = new List<KontorInfo>();
            // Display names of embedded resources
            /*var assembly = typeof(Kontorer).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())  {
                System.Diagnostics.Debug.WriteLine(">>> " + res);
            }*/
            #region Hide
            //Index 0
            #region KK DK XX

            hld = new Holding();

            hld.Start = 0;
            hld.Lenght = 2;

            indexes.Add(hld);

            #endregion
            //Index 1
            #region 

            hld = new Holding();

            hld.Start = 3;
            hld.Lenght = 4;

            indexes.Add(hld);

            #endregion
            //Index 2
            #region Postkode??

            hld = new Holding();

            hld.Start = 8;
            hld.Lenght = 21;

            indexes.Add(hld);

            #endregion
            //Index 3
            #region  Navn

            hld = new Holding();

            hld.Start = 29;
            hld.Lenght = 41;

            indexes.Add(hld);

            #endregion
            //Index 4
            #region Postboks

            hld = new Holding();

            hld.Start = 70;
            hld.Lenght = 36;

            indexes.Add(hld);

            #endregion
            //Index 5
            #region Postboks Spesial

            hld = new Holding();

            hld.Start = 106;
            hld.Lenght = 36;

            indexes.Add(hld);

            #endregion
            //Index 6
            #region Postkode

            hld = new Holding();

            hld.Start = 152;
            hld.Lenght = 36;

            indexes.Add(hld);

            #endregion
            //Index 7
            #region Gate

            hld = new Holding();

            hld.Start = 178;
            hld.Lenght = 72;

            indexes.Add(hld);

            #endregion
            //Index 8
            #region Postboks 2??

            hld = new Holding();

            hld.Start = 250;
            hld.Lenght = 36;

            indexes.Add(hld);

            #endregion
            //Index 9
            #region Nummer1

            hld = new Holding();

            hld.Start = 286;
            hld.Lenght = 9;

            indexes.Add(hld);

            #endregion
            //Index 10
            #region Nummer2

            hld = new Holding();

            hld.Start = 295;
            hld.Lenght = 9;

            indexes.Add(hld);

            #endregion
            //Index 11
            #region Epost

            hld = new Holding();

            hld.Start = 304;
            hld.Lenght = 71;

            indexes.Add(hld);

            #endregion
            //Index 12
            #region Åpnings tider

            hld = new Holding();

            hld.Start = 375;
            hld.Lenght = 123;

            indexes.Add(hld);

            #endregion
            //Index 13
            #region Lat

            hld = new Holding();

            hld.Start = 498;
            hld.Lenght = 10;

            indexes.Add(hld);

            #endregion
            //Index 14
            #region Long

            hld = new Holding();

            hld.Start = 508;
            hld.Lenght = 9;

            indexes.Add(hld);

            #endregion



            #endregion


            index = indexes.ToArray();
            #region Read File
            var assembly = typeof(Kontorer).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("App3.Droid.Kontorer.txt");


            using (var reader = new StreamReader(stream))
            {
                try { 
                    while (reader.Peek() > -1)
                    {
                        string x = reader.ReadLine();
                        string t = x;


                        int res0;

                        string Kontor = "";
                        string Postboks = "";
                        string Addresse = "";

                        string Number1 = ""; int Nummer1 = 0;
                        string Number2 = ""; int Nummer2 = 0;

                        string Epost = "";
                        string Åpning = "";


                        Double latitude = 0; 
                        Double longitude = 0; 
   

                        #region A lot of if's
                        if (t.Length > index[3].Start + index[3].Lenght) 
                        {
                            Kontor = t.Substring(index[3].Start, index[3].Lenght).TrimEnd();
                        }

                        if (t.Length > index[4].Start + index[4].Lenght) 
                        {
                            Postboks = t.Substring(index[4].Start, index[4].Lenght).TrimEnd();
                        }

                        if (t.Length > index[7].Start + index[7].Lenght) 
                        {
                            Addresse = t.Substring(index[7].Start, index[7].Lenght).TrimEnd();
                        }

                        if (t.Length > index[9].Start + index[9].Lenght) 
                        {
                            Number1 = t.Substring(index[9].Start, index[9].Lenght).TrimEnd();
                        

                        
                            bool res = int.TryParse(Number1, out res0);
                            if (res)
                            {
                                Nummer1 = res0;
                            }


                        }

                        if (t.Length > index[10].Start + index[10].Lenght) 
                        {
                            Number2 = t.Substring(index[10].Start, index[10].Lenght).TrimEnd();
                        

                            bool res2 = int.TryParse(Number2, out res0);
                            if (res2)
                            {
                                Nummer2 = res0;
                            }

                        }
                        if (t.Length > index[11].Start + index[11].Lenght) 
                        {
                            Epost = t.Substring(index[11].Start, index[11].Lenght).TrimEnd();
                        }
                        if (t.Length > index[12].Start + index[12].Lenght) 
                        {
                            Åpning = t.Substring(index[12].Start, index[12].Lenght).TrimEnd();
                        }


                        bool latitude_ignore = false;
                        if (t.Length > index[13].Start + index[13].Lenght) 
                        {
                            string t_latitude = t.Substring(index[13].Start, index[13].Lenght).TrimEnd();

                            if (t_latitude == "00.000000" || t_latitude== "" || t_latitude == " ")
                            {
                                latitude_ignore = true;
                            }
                            else if (t_latitude.Length == 0)
                            {
                                latitude_ignore = true;
                            }
                            else
                            {
                                latitude_ignore = false;
                            }

                            try
                            {
                                latitude = Double.Parse(t_latitude, System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }


                        }
                        else { latitude_ignore = true; }

                        bool longitude_ignore = false;
                        if (t.Length >= index[14].Start + index[14].Lenght) 
                        {
                            string t_longitude = t.Substring(index[14].Start, index[14].Lenght).TrimEnd();

                            if (t_longitude == "00.000000" || t_longitude == "" || t_longitude == " ")
                            {
                                longitude_ignore = true;
                            } 
                            else if (t_longitude.Length == 0) 
                            {
                                longitude_ignore = true;
                            }
                            else
                            {
                                longitude_ignore = false;
                            }

                            try
                            {
                                longitude = Double.Parse(t_longitude, System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch { }

                        }
                        else { longitude_ignore = true; }

                        #endregion


                        Console.WriteLine("> " +Kontor);


                        if (Kontor != "")
                        {
                            foreach (string Item in Vis)
                            {
                                if (Kontor.Contains(Item))
                                {
                                    if ((!latitude_ignore) && (!longitude_ignore))
                                    {
                                        Console.WriteLine(">>> " + Kontor);
                                        Kontorinf.Add(new KontorInfo
                                        {
                                            Kontor = Kontor,
                                            Postboks = Postboks,
                                            Addresse = Addresse,
                                            Nummer1 = Nummer1,
                                            Nummer2 = Nummer2,
                                            Epost = Epost,
                                            Åpent = Åpning,
                                            Latitude = latitude,
                                            Longitude = longitude
                                        });

                                    }
                                }
                            }



                        }

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            #endregion



            return Kontorinf;
        }


        
    }

    public class KontorInfo
    {
        public  String Kontor { get; set; }
        public  int Telefon { get; set; }
        public  String Åpent { get; set; }
        public String Addresse { get; set; }
        public String Postboks { get; set; }
        public String PostAddresse { get; set; }
        public int Nummer1 { get; set; }
        public int Nummer2 { get; set; }
        public String Epost { get; set; }
        public  Double Latitude { get; set; }
        public Double Longitude { get; set; }
    }

    public class Holding
    {
        public int Start { get; set; }
        public int Lenght { get; set; }
    }

}
