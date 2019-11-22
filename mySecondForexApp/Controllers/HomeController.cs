using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mySecondForexApp.Models;
using System.Net.Http;
using System.IO;
using System.Data;
using System.Reflection;

namespace mySecondForexApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {

            //----------------------------------------------------------------------------
            //-------------------------------Now API request------------------------------
            HttpClient client = new HttpClient();
            //instanciamos la clase httpclient

            List<RootObject> lista1 = new List<RootObject>();

            RootObject rootObject = null;

            string mainUrl = "http://data.fixer.io/api/";
            string apiKey = "access_key=f65fed759af96fcade0482113ff22d1e";
            List<string> quotesList = new List<string>();

            string dataInit = "2019-10-01";
            DateTime fromDate = Convert.ToDateTime(dataInit);
            string dataEnd = "2019-10-31";
            DateTime toDate = Convert.ToDateTime(dataEnd);
            TimeSpan tSpan = toDate - fromDate;
            int days = tSpan.Days;
            Console.WriteLine("---tSpan---", days);
            for (int i = 0; i <= days; i++)
            {
                fromDate = fromDate.AddDays(1);
                //variable url
                string url = mainUrl + dataInit + "?" + apiKey;
                dataInit = fromDate.ToString("yyyy-MM-dd");


                HttpResponseMessage response = await client.GetAsync(url);
                //response es el nombre de la variable que le damos en este caso
                //Console.WriteLine(response);


                if (response.IsSuccessStatusCode)
                {
                    rootObject = await response.Content.ReadAsAsync<RootObject>();
                    lista1.Add(rootObject);
                    if (i == 0)
                    {
                        quotesList = rootObject.ExcRate.GetNames();
                    }
                }
            }
            List<RootObject> sortedListByDate = lista1.OrderBy(RootObject => RootObject.Date).ToList();
            //tengo un atributo en RootObject que me guarda la lista de Characters
            //lo tengo que recorrer


            //---- finished API here------------------------------------------------------------------------
            //------------------------------- Now .CSV data ------------------------------
            //.csv files
            List<Transaction> values1 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data1.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
            List<Transaction> values2 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data2.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
            List<Transaction> values3 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data3.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
            //return View(values[0]);

            //all data taken from .csv files
            var values = values1.Concat(values2).Concat(values3).OrderBy(x => x.Date).ToList();

            //what I need to find the relationship between both files
            DateTime fecha; //when
            string divisa = ""; //what


            //new obj to store that date-data
            RootObject mySearch = new RootObject();

            //to find exchange rate of each transaction
            foreach (Transaction transaccion in values)
            {
                fecha = transaccion.Date;
                divisa = transaccion.Currency;

                //busco data de la fecha de la transaccion (one line = one transaction)
                mySearch = sortedListByDate.Where(t => t.Date == fecha).FirstOrDefault();
                Console.WriteLine(mySearch);

                if (mySearch != null)
                {
                    //get object structure to access to its atributes
                    var type = mySearch.GetType();

                    /*I got already from the API, the object (line) associated to the date I am searching for.
                    Now---> access to - ExcRate - atribute's values (all currencies)*/
                    Character exchangeRate = (Character)type.GetProperty("ExcRate").GetValue(mySearch);
                    var typeOfChange = exchangeRate.GetType();

                    //find the value of that currency in exchangeRate
                    float transAmount = (float)typeOfChange.GetProperty(divisa).GetValue(exchangeRate);
                    float howMuch = (float)Convert.ToDouble(transaccion.Amount);
                    transaccion.AmountEur = transAmount * howMuch;

                }


            }



            return View(values);
        }


        //public async Task<IActionResult> About()
        //{

        //    //----------------------------------------------------------------------------
        //    //-------------------------------Now API request------------------------------
        //    HttpClient client = new HttpClient();
        //    //instanciamos la clase httpclient

        //    List<RootObject> lista1 = new List<RootObject>();

        //    RootObject rootObject = null;

        //    string mainUrl = "http://data.fixer.io/api/";
        //    string apiKey = "access_key=f65fed759af96fcade0482113ff22d1e";
        //    List<string> quotesList = new List<string>();

        //    string data = "2019-10-01";
        //    DateTime nuevaFecha = Convert.ToDateTime(data);
        //    for (int i=0; i<5; i++)
        //    {
        //    nuevaFecha = nuevaFecha.AddDays(1);
        //        Debug.WriteLine("---data----", data);
        //        Debug.WriteLine("***---nuevaFecha----***", nuevaFecha, "/////");
        //        //variable url
        //        string url = mainUrl + data + "?" + apiKey;
        //        data = nuevaFecha.ToString("yyyy-MM-dd");
        //        Debug.WriteLine("-------", url, "----");

        //        HttpResponseMessage response = await client.GetAsync(url);
        //        //response es el nombre de la variable que le damos en este caso
        //        //Console.WriteLine(response);


        //    if (response.IsSuccessStatusCode)
        //        {
        //            rootObject = await response.Content.ReadAsAsync<RootObject>();
        //            lista1.Add(rootObject);
        //            if (i == 0)
        //            {
        //                quotesList = rootObject.ExcRate.GetNames();
        //            }
        //        }
        //    }
        //    List<RootObject> sortedListByDate = lista1.OrderBy(RootObject => RootObject.Date).ToList();
        //    //tengo un atributo en RootObject que me guarda la lista de Characters
        //    //lo tengo que recorrer
        //    //foreach (Character character in rootObject)
        //    //    //foreach (character character in lista1)
        //    //    //{
        //    //        Console.WriteLine(character);
        //    //        character c = new character
        //    //        {

        //    //            date = character.date,
        //    //            base = character.base,
        //    //            quote = character.quote,
        //    //            rate = character.rate,

        //    //        };
        //    //        lista1.add(c);
        //    //    }
        //    /*
        //    if (rootObject.Next == null)
        //    {
        //        next = false;
        //    }
        //    */
        //    //i++;

        //    //}

        //    //---- finished API here------------------------------------------------------------------------
        //    //------------------------------- Now .CSV data ------------------------------
        //    //.csv files
        //    List<Transaction> values1 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data1.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
        //    List<Transaction> values2 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data2.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
        //    List<Transaction> values3 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data3.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
        //    //return View(values[0]);

        //    //all data taken from .csv files
        //    var values = values1.Concat(values2).Concat(values3).OrderBy(x => x.Date).ToList();

        //    //what I need to find the relationship between both files
        //    DateTime fecha; //when
        //    string divisa = ""; //what


        //    //new obj to store that date-data
        //    RootObject mySearch = new RootObject();

        //    //to find exchange rate of each transaction
        //    foreach (Transaction transaccion in values)
        //    {
        //        fecha = transaccion.Date;
        //        divisa = transaccion.Currency;



        //        //busco data de la fecha de la transaccion (one line = one transaction)
        //        mySearch = sortedListByDate.Where(t => t.Date == fecha).FirstOrDefault();
        //        Console.WriteLine(mySearch);

        //        if(mySearch != null)
        //        {
        //            //get object structure to access to its atributes
        //            var type = mySearch.GetType();

        //            /*I got already from the API, the object (line) associated to the date I am searching for.
        //            Now---> access to - ExcRate - atribute's values (all currencies)*/
        //            Character exchangeRate = (Character)type.GetProperty("ExcRate").GetValue(mySearch);
        //            var typeOfChange = exchangeRate.GetType();

        //            //find the value of that currency in exchangeRate
        //            float transAmount = (float)typeOfChange.GetProperty(divisa).GetValue(exchangeRate);
        //            float howMuch = (float)Convert.ToDouble(transaccion.Amount);
        //            transaccion.AmountEur = transAmount * howMuch;

        //        }


        //    }



        //    return View(sortedListByDate);
        //}

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public async Task<IActionResult> Privacy()
        {

            //----------------------------------------------------------------------------
            //-------------------------------Now API request------------------------------
            HttpClient client = new HttpClient();
            //instanciamos la clase httpclient

            List<RootObject> lista1 = new List<RootObject>();

            RootObject rootObject = null;

            string mainUrl = "http://data.fixer.io/api/";
            string apiKey = "access_key=f65fed759af96fcade0482113ff22d1e";
            List<string> quotesList = new List<string>();

            string data = "2019-10-01";
            DateTime nuevaFecha = Convert.ToDateTime(data);
            for (int i = 0; i < 5; i++)
            {
                nuevaFecha = nuevaFecha.AddDays(1);
                Debug.WriteLine("---data----", data);
                Debug.WriteLine("***---nuevaFecha----***", nuevaFecha, "/////");
                //variable url
                string url = mainUrl + data + "?" + apiKey;
                data = nuevaFecha.ToString("yyyy-MM-dd");
                Debug.WriteLine("-------", url, "----");

                HttpResponseMessage response = await client.GetAsync(url);
                //response es el nombre de la variable que le damos en este caso
                //Console.WriteLine(response);


                if (response.IsSuccessStatusCode)
                {
                    rootObject = await response.Content.ReadAsAsync<RootObject>();
                    lista1.Add(rootObject);
                    if (i == 0)
                    {
                        quotesList = rootObject.ExcRate.GetNames();
                    }
                }
            }
            List<RootObject> sortedListByDate = lista1.OrderBy(RootObject => RootObject.Date).ToList();
            //tengo un atributo en RootObject que me guarda la lista de Characters
            //lo tengo que recorrer


            //---- finished API here------------------------------------------------------------------------
            //------------------------------- Now .CSV data ------------------------------
            //.csv files
            List<Transaction> values1 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data1.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
            List<Transaction> values2 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data2.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
            List<Transaction> values3 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data3.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
            //return View(values[0]);

            //all data taken from .csv files
            var values = values1.Concat(values2).Concat(values3).OrderBy(x => x.Date).ToList();

            //what I need to find the relationship between both files
            DateTime fecha; //when
            string divisa = ""; //what
            string country = "";
            string[] paises = new string[];


            //new obj to store that date-data
            RootObject mySearch = new RootObject();

            //to find exchange rate of each transaction
            foreach (Transaction transaccion in values)
            {
                fecha = transaccion.Date;
                divisa = transaccion.Currency;
                country = transaccion.Country;

                

                

                //busco data de la fecha de la transaccion (one line = one transaction)
                mySearch = sortedListByDate.Where(t => t.Date == fecha).FirstOrDefault();
                Console.WriteLine(mySearch);

                if (mySearch != null)
                {
                    //get object structure to access to its atributes
                    var type = mySearch.GetType();

                    /*I got already from the API, the object (line) associated to the date I am searching for.
                    Now---> access to - ExcRate - atribute's values (all currencies)*/
                    Character exchangeRate = (Character)type.GetProperty("ExcRate").GetValue(mySearch);
                    var typeOfChange = exchangeRate.GetType();

                    //find the value of that currency in exchangeRate
                    float transAmount = (float)typeOfChange.GetProperty(divisa).GetValue(exchangeRate);
                    float howMuch = (float)Convert.ToDouble(transaccion.Amount);
                    transaccion.AmountEur = transAmount * howMuch;

                }


            }
            return View(values);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
