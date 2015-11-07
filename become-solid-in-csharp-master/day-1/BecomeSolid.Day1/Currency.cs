using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BecomeSolid.Day1
{
    public class Currency
    {
        public JObject JoResponse { get; set; }
        public JObject Query { get; set; }
        public JObject Results { get; set; }
        public JObject RateObj { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Rate { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Ask { get; set; }
        public string Bid { get; set; }

        public Currency(string responseString)
        {
            JoResponse = JObject.Parse(responseString);
            Query = (JObject)JoResponse["query"];
            Results = (JObject)Query["results"];
            RateObj = (JObject)Results["rate"];
            Id = (string)RateObj["Id"];
            Name = (string)RateObj["Name"];
            Rate = (string)RateObj["Rate"];
            Date = (string)RateObj["Date"];
            Time = (string)RateObj["Time"];
            Ask = (string)RateObj["Ask"];
            Bid = (string)RateObj["Bid"];
        }

    }
}
