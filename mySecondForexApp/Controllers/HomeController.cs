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
using System.Text;

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

        public async Task<IActionResult> Index(string sortOrder) //funcionalidad de sorting(ordenando) by (id, date, country o amount of EUR...)
        {
            List<TransactionData> transactionsData = await _dataService.GetTransactionsAsync();
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date" : "";

            ViewBag.CountrySortParm = sortOrder == "Country" ? "country_desc" : "Country";
            ViewBag.EurSortParm = sortOrder == "AmountEur" ? "eur_desc" : "AmountEur";

            var objTransaction = from t in transactionsData
                           select t;
            switch (sortOrder) //Parallel ordenarlo
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

            //envio la agrupacion ordenada a la vista
            return View(objTransaction.ToList());
        }

        public async Task<IActionResult> Group() //aqui no estoy ordenando, aqui los estoy filtrando. solo selecciono lso que cumplen condicion
        {
            //clasification in groups
            List<TransactionData> transactionsData = await _dataService.GetTransactionsAsync();

            //create a new list to save clasificated data, to send it to the view
            List<DataByGroup> listByGroups = new List<DataByGroup>();

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
            groupRow.UpdateAmountEuro(RowGroup);
            groupUk.UpdateAmountEuro(UkGroup);
            groupSA.UpdateAmountEuro(SaGroup);
            groupAustra.UpdateAmountEuro(AustraliaGroup);

            //add cada group a listByGroups
            listByGroups.Add(groupEU);
            listByGroups.Add(groupRow);
            listByGroups.Add(groupUk);
            listByGroups.Add(groupSA);
            listByGroups.Add(groupAustra);

            /*export to .CSV*/
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < listByGroups.Count; i++)
            {
                DataByGroup infoGroup = (DataByGroup)listByGroups[i];
                //Append data with separator.
                sb.Append(infoGroup.GroupId + ',' + infoGroup.Group + ',' + infoGroup.AmountEur);

                //Append new line character.
                sb.Append("\r\n");

            }
            System.IO.File.AppendAllText("Grid.csv", sb.ToString());
            return View(listByGroups);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
