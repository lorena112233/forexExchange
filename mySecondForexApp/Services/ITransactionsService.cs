using mySecondForexApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace mySecondForexApp.Services
{
    public interface ITransactionsService
    {
        List<TransactionData> GetTransactions();
        List<TransactionData> PopulateEurField(List<TransactionData> values, List<RootObject> sortedListByDate);
    }
}
