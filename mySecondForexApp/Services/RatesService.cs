using mySecondForexApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace mySecondForexApp.Services
{
    public class RatesService : IRatesService
    {
        //va a devolver una lista de <RootObject> de forma asincrona
        public async Task<List<RootObject>> GetRatesAsync()
        {
            //-------------------------------Now API request------------------------------
            HttpClient client = new HttpClient();
            //instanciamos la clase httpclient

            List<RootObject> lista1 = new List<RootObject>();

            RootObject rootObject = null;

            string mainUrl = "http://data.fixer.io/api/";
            string apiKey = "access_key=00000000000000000";
            List<string> quotesList = new List<string>();

            string dataInit = "2019-10-01";
            DateTime fromDate = Convert.ToDateTime(dataInit);
            string dataEnd = "2019-10-31";
            DateTime toDate = Convert.ToDateTime(dataEnd);
            TimeSpan tSpan = toDate - fromDate;
            int days = tSpan.Days;

            for (int i = 0; i <= days; i++)
            {
                fromDate = fromDate.AddDays(1);
                //variable url
                string url = mainUrl + dataInit + "?" + apiKey;
                dataInit = fromDate.ToString("yyyy-MM-dd");

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    rootObject = await response.Content.ReadAsAsync<RootObject>();
                    lista1.Add(rootObject);
                }
            }

            List<RootObject> sortedListByDate = lista1.OrderBy(RootObject => RootObject.Date).ToList();

            return sortedListByDate;
            //---- finished API here------------------------------------------------------------------------
        }
    }
}
