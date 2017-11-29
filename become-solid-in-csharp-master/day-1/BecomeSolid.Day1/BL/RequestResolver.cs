using BecomeSolid.Day1.DAL;
using BecomeSolid.Day1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace BecomeSolid.Day1.BL
{
	class RequestResolver : IRequestResolve
	{
		public string GetResponceText(string request, Api bot, Update update)
		{
			string[] splitedRequest = request.Split(' ');
			string command = splitedRequest[0].StartsWith("/") ? splitedRequest[0] : "";
			string responseText = "";

			switch (command) {
			case "/start": {
					responseText = "/weather - узнать погоду. \n/task - список задач \n/AI - исскуственный интелект!";
				}
				break;
			case "/weather": {
					switch (splitedRequest.Length) {
					case 1:
						responseText = BuiltWeatherResponce();
						break;
					case 2:
						responseText = BuiltWeatherResponce(splitedRequest[1]);
						break;
						//case 2:responseText = Built___Response(splitedRequest[1], splitedRequest[2]);
						//    break;
					}

				}
				break;
			case "/task":
				responseText = BuiltTaskResponse(request);
				break;
			case "/AI": {
					bot.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
					Task.Delay(2000);
					responseText = BuiltArtifitialIntelegenceResponse(request);
				}

				break;
			}

			return responseText.Equals("") ? "чаво?" : responseText;

		}

		public string BuiltWeatherResponce(string city = "Minsk", string date = "")
		{
			if (date.Equals(""))
				date = DateTime.Now.ToString();

			///TODO date    
			WebUtility.UrlEncode(city);
			var weatherApiKey = "ec259b32688dc1c1d087ebc30cbff9ed";
			string url =
				string.Format(
					"http://api.openweathermap.org/data/2.5/weather?q={0}&APPID={1}&units=metric",
					city, weatherApiKey);

			WebRequest request = WebRequest.Create(url);
			WebResponse response = request.GetResponse();

			using (var streamReader = new StreamReader(response.GetResponseStream())) {
				string responseString = streamReader.ReadToEnd();

				Console.WriteLine(responseString);

				JObject joResponse = JObject.Parse(responseString);
				JObject main = (JObject) joResponse["main"];
				double temp = (double) main["temp"];
				JObject weather = (JObject) joResponse["weather"][0];
				string description = (string) weather["description"];
				string cityName = (string) joResponse["name"];

				Console.WriteLine(string.Format("temp is: {0}", temp));

				var message = "In " + cityName + " " + description + " and the temperature is " +
							  temp.ToString("+#;-#") + "°C";

				Console.WriteLine("Echo Message: {0}", message);

				return message;
			}
		}

		public string BuiltTaskResponse(string taskRequest)
		{
			{
				StringBuilder taskResponse = new StringBuilder("");

				//using (TaskContext taskContext = new TaskContext()) {
				var messageParts = taskRequest.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				if (messageParts.Length > 1) {
					string taskCommand = messageParts[1];

					switch (taskCommand) {
					case "clear": {
							using (TaskContext taskContext = new TaskContext()) {
								//IEnumerable<TaskModel> taskModels = taskContext.Tasks; 
								foreach (var task in taskContext.ToDos) {
									taskContext.ToDos.Remove(task);
								}
								//taskContext.Tasks.RemoveRange(taskContext.Tasks); 
								taskContext.SaveChanges();
								taskResponse.Append("Список заданий очищен.");
							}
						}
						break;
					//сделать через "список"
					case "list": {
							using (TaskContext taskContext = new TaskContext()) {
								foreach (var task in taskContext.ToDos) {
									taskResponse.Append(task.CreationtionDate);
									taskResponse.Append(" ");
									taskResponse.Append(task.Description);
									taskResponse.Append("\n");
								}

								if (taskContext.ToDos.Count() == 0)
									taskResponse.Append("у вас нет напоминаний");
							}
						}
						break;
					//сделать через "напомни ..."
					case "add": {
							if (messageParts.Length > 2) {
								string taskText = taskRequest.Remove(0, 9);

								//if (DateTime.TryParse(messageParts[2], out dateTime)) {
								//	//messageParts.Last().Remove(messageParts.Last().Length-1, 1); 
								//	string forDebug = string.Join(" ", messageParts);
								//	taskContext.Tasks.Add(new TaskModel() { DateTime = dateTime, Description = string.Join(" ", messageParts) });
								//	taskContext.SaveChanges();
								//}
								using (TaskContext taskContext = new TaskContext()) {
									taskContext.ToDos.Add(new ToDo() { CreationtionDate = DateTime.Now, Description = taskText });
									taskContext.SaveChanges();
									taskResponse.Append($"({taskText}) добавлено в список дел.");
								}
							}
						}
						break;
					}
				} else
					return "/task clear - очистить список задач\n/task list - вывести список напоминаний\n/task add {что напомнить}- добавить напоминание";
				//}

				return taskResponse.ToString();
			}
		}

		public string BuiltCurrencyResponse(string taskRequest)
		{
			var messageParts = taskRequest.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			var kindCurrency = messageParts.Length == 1 ? "USDBYR" : messageParts.Skip(1).First();

			WebUtility.UrlEncode(kindCurrency);

			string url =
				string.Format(
					"https://query.yahooapis.com/v1/public/yql?q=select+*+from+yahoo.finance.xchange+where+pair+=+%22{0}%22&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=",
					kindCurrency);

			WebRequest request = WebRequest.Create(url);
			WebResponse response = request.GetResponse();

			using (var streamReader = new StreamReader(response.GetResponseStream())) {
				string responseString = streamReader.ReadToEnd();

				Console.WriteLine(responseString);

				Currency currency = new Currency(responseString);

				Console.WriteLine(string.Format("{0} is: {1} ", currency.Id, currency.Rate));

				var message = "Курс " + currency.Name + " Дата " + currency.Date + " Время " +
							  currency.Time + " Покупка " + currency.Ask + " Продажа " +
							  currency.Bid;
				//Console.WriteLine("Echo Message: {0}", message);
				return message;
			}
		}


		public string BuiltArtifitialIntelegenceResponse(string taskRequest)
		{
			if (taskRequest.Contains("какая сейчас погода?")) {
				return BuiltWeatherResponce();
			} else
				if (taskRequest.Contains("какой сейчас курс?")) {
				return BuiltCurrencyResponse(taskRequest);
			} else {
				StringBuilder taskResponse = new StringBuilder("");

				using (BotContext botContext = new BotContext()) {
					string[] tags = taskRequest.ToLower().Split();
					string ansver = "";

					//IEnumerable<Ansver> ansvers = botContext.Ansvers;
					//foreach (var tag in tags) {
					//	ansvers.Where(w => w.Value.Contains(tag));
					//	if (ansvers.Count() != 0) {
					//		ansver += ansvers.First().Value;
					//		break;
					//	}
					//}

					if (ansver.Equals("")) {
						taskResponse.Append("к такому я не был готов");
					} else {
						taskResponse.Append(ansver);
					}
				}

				return taskResponse.ToString();
			}
		}
	}
}
