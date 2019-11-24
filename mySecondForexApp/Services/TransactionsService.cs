﻿using mySecondForexApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mySecondForexApp.Services
{
    public class TransactionsService : ITransactionsService
    {
        public List<TransactionData> GetTransactions()
        {
            //------------------------------- Now .CSV data ------------------------------
            //.csv files
            List<TransactionData> values1 = System.IO.File.ReadAllLines("data1.csv").Skip(1).Select(v => TransactionData.FromCsv(v)).ToList();
            List<TransactionData> values2 = System.IO.File.ReadAllLines("data2.csv").Skip(1).Select(v => TransactionData.FromCsv(v)).ToList();
            List<TransactionData> values3 = System.IO.File.ReadAllLines("data3.csv").Skip(1).Select(v => TransactionData.FromCsv(v)).ToList();

            //all data taken from .csv files
            List<TransactionData> values = values1.Concat(values2).Concat(values3).OrderBy(x => x.Date).ToList();

            return values;
        }

        //merge info from .csv and API
        public List<TransactionData> PopulateEurField(List<TransactionData> values, List<RootObject> sortedListByDate)
        {
            //what I need to find the relationship between both files
            DateTime fecha; //when
            string divisa = ""; //what


            //new obj to store that date-data
            RootObject mySearch = new RootObject();

            //to find exchange rate of each transaction
            foreach (TransactionData transaccion in values)
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

                    /*I got already from the API, the object associated to the date I am searching for.
                    Now---> access to - ExcRate - atribute's values (all currencies)*/
                    Character exchangeRate = (Character)type.GetProperty("ExcRate").GetValue(mySearch);
                    var typeOfChange = exchangeRate.GetType();

                    //find the value of that currency in exchangeRate
                    float rate = (float)typeOfChange.GetProperty(divisa).GetValue(exchangeRate);
                    float amount = (float)Convert.ToDouble(transaccion.Amount);
                    //AmountEur can be derived by dividing the 'amount' by 'rate'.
                    transaccion.AmountEur = amount / rate;
                    //EU group
                    float totalEU = 0;
                    var EUGroup =
                        from transac2 in values
                        where transac2.Country == "Austria" || transac2.Country == "Italy" || transac2.Country == "Belgium" || transac2.Country == "Latvia"
                        select transac2;


                    foreach (TransactionData transaction in EUGroup)
                    {
                        totalEU += transaction.AmountEur;
                        transaction.Group = "EU";
                    }

                }

            } //end foreach

            return values;
        }

        //clasification / groups by countries
        public List<TransactionData> PopulateGroupField(List<TransactionData> values)
        {

            /*  Austria, Italy, Belgium and Latvia = 'EU'
                Chile, Qatar, United Arab Emirates and United States of America = 'ROW'
                United Kingdom = 'United Kingdom', 
                Australia = 'Australia'
                South Africa = 'South Africa'
            */

            //List<CountryGroup> groupCountry = new List<CountryGroup>();

            //EU group
            float totalEU = 0;
            var EUGroup =
                from transaccion in values
                where transaccion.Country == "Austria" || transaccion.Country == "Italy" || transaccion.Country == "Belgium" || transaccion.Country == "Latvia"
                select transaccion;


            foreach (TransactionData transaction in EUGroup)
            {
                totalEU += transaction.AmountEur;
                transaction.Group = "EU";
            }


            //
            return values;
        }

    }
}
