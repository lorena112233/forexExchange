using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mySecondForexApp.Models
{
    public class DataByGroup
    {
        public int GroupId { get; set; }
        public string Group { get; set; }
        public decimal AmountEur { get; set; }

        public void addAmount(decimal amount)
        {
            this.AmountEur += amount;
        }

        public DataByGroup()
        {

        }

        //contructor con parámetros
        public DataByGroup(int GroupId, string Group, decimal AmountEur)
        {
            this.GroupId = GroupId;
            this.Group = Group;
            this.AmountEur = AmountEur;
        }

        public void NewTransaction(decimal amount)
        {
            AmountEur = AmountEur + amount;
        }

        //I get a list of an specific group, take their list of transactions to sum them all in EUR 
        public void UpdateAmountEuro(IEnumerable<TransactionData> groupTransactions)
        {
            foreach (TransactionData transaction in groupTransactions)
            {
                this.AmountEur += (decimal)transaction.AmountEur;
            }
        }


        //public static DataByGroup CalculateAmountEur(List<TransactionData> mergedResults)
        //{

        //    //foreach (TransactionData transaccion in mergedResults)
        //    //{

        //    //}

        //}


    }
}
