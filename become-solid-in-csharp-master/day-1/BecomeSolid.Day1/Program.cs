using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace BecomeSolid.Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().Wait();
        }

        static async Task Run()
        {
            var bot = new Api("127266371:AAHoh0eh4Nt0mzB1LOoZwg8E7vQqeoMswuI");
            
            var me = await bot.GetMe();

            Console.WriteLine("Hello my name is {0}", me.Username);

            var offset = 0;

            while (true)
            {
                var updates = await bot.GetUpdates(offset);

                foreach (var update in updates)
                {
                    if (update.Message.Type == MessageType.TextMessage)
                    {
                        var inputMessage = update.Message.Text;

                        if (inputMessage.StartsWith("/currency"))
                        {
                            var messageParts = inputMessage.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            var kindCurrency = messageParts.Length == 1 ? "USDBYR" : messageParts.Skip(1).First();
                            WebUtility.UrlEncode(kindCurrency);
                            
                            string url = string.Format("https://query.yahooapis.com/v1/public/yql?q=select+*+from+yahoo.finance.xchange+where+pair+=+%22{0}%22&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=", kindCurrency);
                            WebRequest request = WebRequest.Create(url);
                            WebResponse response = request.GetResponse();
                            using (var streamReader = new StreamReader(response.GetResponseStream()))
                            {
                                string responseString = streamReader.ReadToEnd();

                                Console.WriteLine(responseString);
                                JObject joResponse = JObject.Parse(responseString);
                                JObject query = (JObject)joResponse["query"];
                                JObject results = (JObject)query["results"];
                                JObject rateObj = (JObject)results["rate"];
                                string id = (string)rateObj["Id"];
                                string name = (string)rateObj["Name"];
                                string rate = (string)rateObj["Rate"];
                                string date = (string)rateObj["Date"];
                                string time = (string)rateObj["Time"];
                                string ask = (string)rateObj["Ask"];
                                string bid = (string)rateObj["Bid"];

                                Console.WriteLine(string.Format("{0} is: {1} ", id, rate));

                                var message = "Курс " + name + "Дата " +date + "Время " + time + "Покупка " + ask + "Продажа " + bid;

                                var t = await bot.SendTextMessage(update.Message.Chat.Id, message);
                                Console.WriteLine("Echo Message: {0}", message);
                            }
                        }
                        else
                        if (inputMessage.StartsWith("/weather"))
                        {
                            var messageParts = inputMessage.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            var city = messageParts.Length == 1 ? "Minsk" : messageParts.Skip(1).First();
                            WebUtility.UrlEncode(city);
                            var weatherApiKey = "ec259b32688dc1c1d087ebc30cbff9ed";
                            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&APPID={1}&units=metric", city, weatherApiKey);
                            WebRequest request = WebRequest.Create(url);
                            WebResponse response = request.GetResponse();
                            using (var streamReader = new StreamReader(response.GetResponseStream()))
                            {
                                string responseString = streamReader.ReadToEnd();

                                Console.WriteLine(responseString);
                                JObject joResponse = JObject.Parse(responseString);
                                JObject main = (JObject)joResponse["main"];
                                double temp = (double)main["temp"];
                                JObject weather = (JObject)joResponse["weather"][0];
                                string description = (string)weather["description"];
                                string cityName = (string)joResponse["name"];

                                Console.WriteLine(string.Format("temp is: {0}", temp));

                                var message = "In " + cityName + " " + description + " and the temperature is " + temp.ToString("+#;-#") + "°C";


                                Console.WriteLine(string.Format("temp is: {0}", temp));


                                var t = await bot.SendTextMessage(update.Message.Chat.Id, message);
                                Console.WriteLine("Echo Message: {0}", message);
                            }
                        }
                        else
                        {
                            await bot.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
                            await Task.Delay(2000);
                            var t = await bot.SendTextMessage(update.Message.Chat.Id, update.Message.Text);
                            Console.WriteLine("Echo Message: {0}", update.Message.Text);
                        }
                    }

                    offset = update.Id + 1;
                }

                await Task.Delay(1000);
            }
        }
    }
}
