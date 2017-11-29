using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BecomeSolid.Day1.DAL;
using BecomeSolid.Day1.Models;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;
using BecomeSolid.Day1.BL;

namespace BecomeSolid.Day1
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			try {
				IRequestResolve requestResolver = new RequestResolver();
				Run(requestResolver).Wait();
			} 
			catch (Exception ex) {

			}
		}

		private static async Task Run(IRequestResolve requestResolver)
		{
			var bot = new Api("454883056:AAFWlNt8oyeQouMasGb9JW824YGGk2c6znA");

			var me = await bot.GetMe();

			Console.WriteLine("Hello my name is {0}", me.Username);

			var offset = 0;

			while (true) {
				var updates = await bot.GetUpdates(offset);

				foreach (var update in updates) {
					if (update.Message?.Type == MessageType.TextMessage) {
						string responseText = requestResolver.GetResponceText(update.Message.Text, bot, update);
						var t = await bot.SendTextMessage(update.Message.Chat.Id, responseText);
						Console.WriteLine(responseText);
						//requestResolver.GetResponceText(update.Message.Text);
					}

					//    var inputMessage = update.Message.Text;
					//    if (inputMessage.StartsWith("/task"))
					//    {

					//    }
					//    //else
					//    //if (inputMessage.StartsWith("/learn"))
					//    //{
					//    //    using (BotContext botContext = new BotContext())
					//    //    {
					//    //        botContext.Ansvers.Add(new Ansver() { Value = "мои дела хорошо, а твои?" });
					//    //        botContext.Ansvers.Add(new Ansver() { Value = "" });
					//    //        botContext.Ansvers.Add(new Ansver() { Value = update.Message.Text.Remove(0, 7) });
					//    //        botContext.SaveChanges();
					//    //        IEnumerable<Ansver> allAnsvers = botContext.Ansvers;
					//    //    }
					//    //}
					//    else
					//    if (inputMessage.StartsWith("/ai"))
					//    {
					//        await bot.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
					//        await Task.Delay(2000);
					//        using (BotContext botContext = new BotContext())
					//        {
					//            string[] tags = update.Message.Text.ToLower().Split();
					//            string ansver = "";
					//            IEnumerable<Ansver> ansvers = botContext.Ansvers;
					//            foreach (var tag in tags)
					//            {
					//                ansvers.Where(w => w.Value.Contains(tag));
					//                if (ansvers.Count() != 0)
					//                {
					//                    ansver += ansvers.First().Value;
					//                    break;
					//                }
					//            }

					//            if (ansver.Equals(""))
					//            {
					//                var t = await bot.SendTextMessage(update.Message.Chat.Id, "к такому я не был готов");
					//            }
					//            else
					//            {
					//                var t = await bot.SendTextMessage(update.Message.Chat.Id, ansver);
					//            }

					//        }

					//        Console.WriteLine("Echo Message: {0}", update.Message.Text);
					//    }
					//    else if (inputMessage.StartsWith("/currency"))
					//    {
					//        var messageParts = inputMessage.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					//        var kindCurrency = messageParts.Length == 1 ? "USDBYR" : messageParts.Skip(1).First();
					//        WebUtility.UrlEncode(kindCurrency);

					//        string url =
					//            string.Format(
					//                "https://query.yahooapis.com/v1/public/yql?q=select+*+from+yahoo.finance.xchange+where+pair+=+%22{0}%22&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=",
					//                kindCurrency);
					//        WebRequest request = WebRequest.Create(url);
					//        WebResponse response = request.GetResponse();
					//        using (var streamReader = new StreamReader(response.GetResponseStream()))
					//        {
					//            string responseString = streamReader.ReadToEnd();

					//            Console.WriteLine(responseString);

					//            Currency currency = new Currency(responseString);

					//            Console.WriteLine(string.Format("{0} is: {1} ", currency.Id, currency.Rate));

					//            var message = "Курс " + currency.Name + " Дата " + currency.Date + " Время " +
					//                          currency.Time + " Покупка " + currency.Ask + " Продажа " +
					//                          currency.Bid;

					//            var t = await bot.SendTextMessage(update.Message.Chat.Id, message);
					//            Console.WriteLine("Echo Message: {0}", message);
					//        }
					//    }
					//    else if (inputMessage.StartsWith("/weather"))
					//    {
					//        var messageParts = inputMessage.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					//        var city = messageParts.Length == 1 ? "Minsk" : messageParts.Skip(1).First();
					//        WebUtility.UrlEncode(city);
					//        var weatherApiKey = "ec259b32688dc1c1d087ebc30cbff9ed";
					//        string url =
					//            string.Format(
					//                "http://api.openweathermap.org/data/2.5/weather?q={0}&APPID={1}&units=metric",
					//                city, weatherApiKey);
					//        WebRequest request = WebRequest.Create(url);
					//        WebResponse response = request.GetResponse();
					//        using (var streamReader = new StreamReader(response.GetResponseStream()))
					//        {
					//            string responseString = streamReader.ReadToEnd();

					//            Console.WriteLine(responseString);
					//            JObject joResponse = JObject.Parse(responseString);
					//            JObject main = (JObject)joResponse["main"];
					//            double temp = (double)main["temp"];
					//            JObject weather = (JObject)joResponse["weather"][0];
					//            string description = (string)weather["description"];
					//            string cityName = (string)joResponse["name"];

					//            Console.WriteLine(string.Format("temp is: {0}", temp));

					//            var message = "In " + cityName + " " + description + " and the temperature is " +
					//                          temp.ToString("+#;-#") + "°C";

					//            var t = await bot.SendTextMessage(update.Message.Chat.Id, message);
					//            Console.WriteLine("Echo Message: {0}", message);
					//        }
					//    }
					//    else
					//    {
					//        await bot.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
					//        await Task.Delay(2000);
					//        var t = await bot.SendTextMessage(update.Message.Chat.Id, update.Message.Text);
					//        Console.WriteLine("Echo Message: {0}", update.Message.Text);
					//    }
					//}

					offset = update.Id + 1;
				}

				await Task.Delay(1000);
			}
		}
	}
}