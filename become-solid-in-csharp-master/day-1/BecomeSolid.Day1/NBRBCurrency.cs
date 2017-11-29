using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BecomeSolid.Day1
{
	public class ApilayerCurrency
	{
		//public JObject JoResponse { get; set; }
		//public JObject Query { get; set; }
		//public JObject Results { get; set; }
		//public JObject RateObj { get; set; }
		//public string Id { get; set; }
		//public string Name { get; set; }
		//public string Rate { get; set; }
		//public string Date { get; set; }
		//public string Time { get; set; }
		//public string Ask { get; set; }
		//public string Bid { get; set; }
		public JObject JoResponse { get; set; }
		public bool Success { get; set; }
		public string Terms { get; set; }
		public string Privacy { get; set; }
		public int TimeStamp { get; set; }
		public string Source { get; set; }
		public JObject Quotes { get; set; }
		public decimal USDBYN { get; set; }
		public ApilayerCurrency(string responseString)
		{
			JoResponse = JObject.Parse(responseString);
			Success = (bool) JoResponse["success"];
			Terms = (string) JoResponse["terms"];
			Privacy = (string) JoResponse["privacy"];
			TimeStamp = (int) JoResponse["timestamp"];
			Source = (string) JoResponse["source"];
			Quotes = (JObject) JoResponse["quotes"];
			USDBYN = (decimal) Quotes["USDBYN"];
		}

	}
}
