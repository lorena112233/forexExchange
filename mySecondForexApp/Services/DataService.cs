using mySecondForexApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mySecondForexApp.Services
{
    public class DataService : IDataService
    {
        private readonly IRatesService _ratesService;
        private readonly ITransactionsService _transactionsService;

        //constructor: inject an object(DataService) toda la funcionalidad de obtener los datos en una sola ubicacion
        public DataService(IRatesService ratesService, ITransactionsService transactionsService)
        {
            _ratesService = ratesService;
            _transactionsService = transactionsService;
        }

        public async Task<List<TransactionData>> GetTransactionsAsync()
        {

            //obtain data from API
            List<RootObject> sortedListByDate = await _ratesService.GetRatesAsync();

            //obtain data from .CSV files
            List<TransactionData> values = _transactionsService.GetTransactions();

            //merge API + .csv files
            List<TransactionData> mergedResults = _transactionsService.PopulateEurField(values, sortedListByDate);

            return mergedResults;
        }

    }
}
