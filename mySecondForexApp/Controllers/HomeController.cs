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
using mySecondForexApp.Services;

namespace mySecondForexApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataService _dataService; 

        //constructor: inject an object(DataService)
        public HomeController(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IActionResult> Index(string sortOrder)
        {
            /*ASI estaba bien sin filtrar
            List<TransactionData> transactionsData = await _dataService.GetTransactionsAsync();

            return View(transactionsData.ToList());
            */

            List<TransactionData> transactionsData = await _dataService.GetTransactionsAsync();
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date" : "";

            ViewBag.CountrySortParm = sortOrder == "Country" ? "country_desc" : "Country";
            ViewBag.EurSortParm = sortOrder == "AmountEur" ? "eur_desc" : "AmountEur";

            var objTransaction = from t in transactionsData
                           select t;
            switch (sortOrder)
            {
                case "Date":
                    objTransaction = objTransaction.OrderBy(t => t.Date);
                    break;

                case "Country":
                    objTransaction = objTransaction.OrderBy(t => t.Country);
                    break;
                case "country_desc":
                    objTransaction = objTransaction.OrderByDescending(t => t.Country);
                    break;


                case "AmountEur":
                    objTransaction = objTransaction.OrderBy(t => t.AmountEur);
                    break;
                case "eur_desc":
                    objTransaction = objTransaction.OrderByDescending(t => t.AmountEur);
                    break;


                default:
                    objTransaction = objTransaction.OrderByDescending(t => t.Date);
                    break;
            }

            //return View(transactionsData.ToList());
            return View(objTransaction.ToList());
        }

        public async Task<IActionResult> Group()
        {
            //clasification in groups
            List<TransactionData> transactionsData = await _dataService.GetTransactionsAsync();

            //create a new list to save clasificated data, to send it to the view
            List<DataByGroup> listByGroups = new List<DataByGroup>();

            //var checkDuplicates =
            //    from r in transactionsData
            //    group r by r.Country into grupedQuery
            //    select new
            //    {
            //        //return number of different countries
            //        Country = grupedQuery.Key,
            //        Count = (from r in grupedQuery select r.Date).Distinct().Count()
            //    };

            ////how many countries
            //int contador = checkDuplicates.Count();

            //string[] paises = new string[0];

            //paises.Append("1111");

            //foreach (var countryLoop in checkDuplicates)
            //{
            //    paises.Append(countryLoop.Country);
            //}

            //string[] countries = new string[contador] { "Argentina", "Bolivia", "Peru", "Chile", "Colombia" };

            //EU group
            //float totalEU = 0;
            //for (int i = 0; i < transactionsData.; i++)
            //{
            //DataByGroup groupEU = new DataByGroup();

            //}
            DataByGroup groupEU = new DataByGroup(1, "EU", 0);
            DataByGroup groupRow = new DataByGroup(2, "ROW", 0);
            DataByGroup groupUk = new DataByGroup(3, "United Kingdom", 0);
            DataByGroup groupSA = new DataByGroup(4, "South Africa", 0);
            DataByGroup groupAustra = new DataByGroup(5, "Australia ", 0);

            var EUGroup = //Define which countries are part of this group
                from transacEU in transactionsData //transactionsData = values
                where transacEU.Country == "Austria" || transacEU.Country == "Italy" || transacEU.Country == "Belgium" || transacEU.Country == "Latvia"
                select transacEU;

            var RowGroup = //Define which countries are part of this group
                from transacRow in transactionsData //transactionsData = values
                where transacRow.Country == "Chile" || transacRow.Country == "Qatar" || transacRow.Country == "United Arab Emirates" || transacRow.Country == "United States of America"
                select transacRow;

            var UkGroup = //Define which countries are part of this group
                from transacUk in transactionsData //transactionsData = values
                where transacUk.Country == "United Kingdom"
                select transacUk;

            var SaGroup = //Define which countries are part of this group
                from transacSa in transactionsData //transactionsData = values
                where transacSa.Country == "South Africa"
                select transacSa;

            var AustraliaGroup = //Define which countries are part of this group
                from transacAustra in transactionsData //transactionsData = values
                where transacAustra.Country == "Australia"
                select transacAustra;

            //after defining which countries belong to each group, sum all transaction amounts:
            groupEU.UpdateAmountEuro(EUGroup);

            listByGroups.Add(groupEU);

            return View(listByGroups);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
