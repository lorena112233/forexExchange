using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mySecondForexApp.Models
{
    public class DataByGroup
    {
        private int GroupId { get; set; }
        private string Group { get; set; }
        private float AmountEur { get; set; }

        private void  addAmount(float amount)
        {
            this.AmountEur += amount;
        }

        public DataByGroup()
        {

        }

        //contructor con parámetros
        public DataByGroup(int GroupId, string Group, float AmountEur)
        {
            this.GroupId = GroupId;
            this.Group = Group;
            this.AmountEur = AmountEur;
        }

        public void NewTransaction(float amount)
        {
            AmountEur = AmountEur + amount;
        }


        //public static DataByGroup CalculateAmountEur(List<TransactionData> mergedResults)
        //{

        //    //foreach (TransactionData transaccion in mergedResults)
        //    //{

        //    //}

        //}

        
    }
}
