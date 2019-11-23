using mySecondForexApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mySecondForexApp.Services
{
    public interface IRatesService
    {
        Task<List<RootObject>> GetRatesAsync();
    }
}
