using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mySecondForexApp.Models
{
    public class Country
    {
        public DateTime date { get; set; }
        public string name { get; set; }
        public float amountEur { get; set; }

        public Country (DateTime date, string name, float amountEur)
        {
            this.date = date;
            this.name = name;
            this.amountEur = amountEur;
        }

        public void newTransaction()
        {

        }

    }
}
