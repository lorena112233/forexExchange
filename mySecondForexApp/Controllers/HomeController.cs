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
            /*ASI estaba bien
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

        //public async Task<IActionResult> UploadedData()
        //{
        //    //
        //    List<TransactionData> transactionsData = await _dataService.GetTransactionsAsync();

        //    return View(transactionsData);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
