using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BecomeSolid.Day1
{
	public class BitcoinCurrency
	{
		public JArray JoResponse { get; set; }
		public decimal BitcoinUSD { get; set; }

		public BitcoinCurrency(string responseString)
		{
			try {
				JoResponse = JArray.Parse(responseString);
				BitcoinUSD = (decimal) JoResponse[0]["price_usd"];
			} catch (Exception ex) {

			}

		}
	}
}
