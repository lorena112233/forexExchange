using mySecondForexApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mySecondForexApp.Services
{
    public interface IDataService
    {
        Task<List<TransactionData>> GetTransactionsAsync();
    }
}
