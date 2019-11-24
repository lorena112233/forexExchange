using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mySecondForexApp.Models
{
    public class RootObject
    {
        public string Success { get; set; }
        public int Timestamp { get; set; }
        public string Base { get; set; }
        public DateTime Date { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "rates")]

        //An obj with all exchange rates as atributes
        public Character ExcRate { get; set; }
    }
}
