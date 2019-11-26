using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace mySecondForexApp.Models
{
    public class DataByGroup
    {
        public int GroupId { get; set; }
        public string Group { get; set; }
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:C}")]
        public decimal AmountEur { get; set; }

        //contructor con parámetros
        public DataByGroup(int GroupId, string Group, decimal AmountEur)
        {
            this.GroupId = GroupId;
            this.Group = Group;
            this.AmountEur = AmountEur;
        }

        //I get a list of an specific group, take their list of transactions to sum them all in EUR 
        public void UpdateAmountEuro(IEnumerable<TransactionData> groupTransactions)
        {
            foreach (TransactionData transaction in groupTransactions)
            {
                this.AmountEur += (decimal)transaction.AmountEur;
            }
        }

    }
}
