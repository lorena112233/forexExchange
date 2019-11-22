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
        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> About()
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
            for (int i=0; i<5; i++)
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
                    quotesList = rootObject.ExcRate.GetNames();
                }
            }
            List<RootObject> sortedListByDate = lista1.OrderBy(RootObject => RootObject.Date).ToList();
            //tengo un atributo en RootObject que me guarda la lista de Characters
            //lo tengo que recorrer
            //foreach (Character character in rootObject)
            //    //foreach (character character in lista1)
            //    //{
            //        Console.WriteLine(character);
            //        character c = new character
            //        {

            //            date = character.date,
            //            base = character.base,
            //            quote = character.quote,
            //            rate = character.rate,

            //        };
            //        lista1.add(c);
            //    }
            /*
            if (rootObject.Next == null)
            {
                next = false;
            }
            */
            //i++;

            //}

            //---- finished API here------------------------------------------------------------------------
            //------------------------------- Now .CSV data ------------------------------
            //.csv files
            List<Transaction> values1 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data1.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
            List<Transaction> values2 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data2.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
            List<Transaction> values3 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data3.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
            //return View(values[0]);

            //all data taken from .csv files
            var values = values1.Concat(values2).Concat(values3).OrderBy(x => x.Date).ToList();
            DateTime fecha;
            string divisa = "";
            string atributo = "";
            RootObject mySearch = new RootObject();

            PropertyInfo[] properties = typeof(RootObject).GetProperties();

            foreach (Transaction transaccion in values)
            {
                fecha = transaccion.Date;
                divisa = transaccion.Currency;
                mySearch = sortedListByDate.Where(t => t.Date == fecha).FirstOrDefault();
                atributo = "ExcRate." + divisa;
                
                var type = mySearch.GetType();
                float cambio = (float)type.GetProperty(rootObject.ExcRate).GetValue(mySearch);

            }



            return View(sortedListByDate);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            //List<Transaction> values = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data1.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();

            //Create a DataTable.  
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Date", typeof(string)),
            new DataColumn("Country", typeof(string)),
            new DataColumn("Currency", typeof(string)),
            new DataColumn("Amount",typeof(string)) });

            //Read the contents of CSV file.  
            string csvData = System.IO.File.ReadAllText("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data1.csv");

            //Execute a loop over the rows.  
            foreach (string row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    dt.Rows.Add();
                    int i = 0;

                    //Execute a loop over the columns.  
                    foreach (string cell in row.Split(','))
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = cell;
                        i++;
                    }
                }
            }

            //Bind the DataTable.  
            //GridView1.DataSource = dt;
            //GridView1.DataBind();


            return View(csvData);
        }

        public IActionResult Privacy()
        {

            List<Transaction> values1 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data1.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
            List<Transaction> values2 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data2.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
            List<Transaction> values3 = System.IO.File.ReadAllLines("D:\\Datos\\Downloads\\1573460310_CITechnicalTest\\CITechnicalTest\\data\\data3.csv").Skip(1).Select(v => Transaction.FromCsv(v)).ToList();
            //return View(values[0]);

            //all data taken from .csv files
            var values = values1.Concat(values2).Concat(values3).OrderBy(x => x.Date).ToList();
            

            ViewData["MyTrans"] = values;

            return View(values);
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
