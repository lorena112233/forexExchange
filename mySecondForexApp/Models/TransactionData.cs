using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mySecondForexApp.Models
{
    public class TransactionData
    {
            public DateTime Date { get; set; }
            public string Country { get; set; }
            public string Currency { get; set; }
            public int Amount { get; set; }
            public decimal AmountEur { get; set; }
            public string Group { get; set; }


        public static TransactionData FromCsv(string csvLine)
            {
                string[] values = csvLine.Split(',');
                TransactionData transactions = new TransactionData();
                transactions.Date = Convert.ToDateTime(values[0]);
                transactions.Country = Convert.ToString(values[1]);
                transactions.Currency = Convert.ToString(values[2]);
                transactions.Amount = Convert.ToInt32(values[3]);
                transactions.AmountEur = 0;
                transactions.Group = "";
                return transactions;
            }
        
    }
}
