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

        public async Task<IActionResult> Index()
        {
            //
            List<TransactionData> transactionsData = await _dataService.GetTransactionsAsync();

            return View(transactionsData);
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
