using BecomeSolid.Day1.DAL;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Xml;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using log4net.Repository.Hierarchy;
using log4net;
using log4net.Appender;

namespace BecomeSolid.Day1.BL
{
	class RequestResolver : IRequestResolve
	{
		private log4net.ILog _log;

		public RequestResolver(log4net.ILog log)
		{
			_log = log;
		}

		public string GetResponceText(string request, Api bot, Update update)
		{
			Console.WriteLine($"date time - {DateTime.Now},request - {request}");
			_log.Info($"date time - {DateTime.Now}, request - {request}");

			string[] splitedRequest = request.Split(' ');
			string command = splitedRequest[0].StartsWith("/") ? splitedRequest[0] : "";
			string responseText = "";

			switch (command.ToLower()) {
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
			case "/todo":
				responseText = BuiltTaskResponse(request);
				break;
			//case "/AI": {
			default: {
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

				Console.WriteLine($"response - {responseString}");
				_log.Info($"date time - {DateTime.Now}, response - {responseString}");

				JObject joResponse = JObject.Parse(responseString);
				JObject main = (JObject) joResponse["main"];
				double temp = (double) main["temp"];
				JObject weather = (JObject) joResponse["weather"][0];
				string description = (string) weather["description"];
				string cityName = (string) joResponse["name"];

				Console.WriteLine(string.Format("temp is: {0}", temp));
				_log.Info(string.Format("date time - {DateTime.Now}, temp is: {0}", temp));

				var message = "In " + cityName + " " + description + " and the temperature is " +
							  temp.ToString("+#;-#") + "°C";

				Console.WriteLine("Echo Message: {0}", message);
				_log.Info($"date time - {DateTime.Now}, Echo Message: {message}");

				return message;
			}
		}

		public string BuiltTaskResponse(string toDoRequest)
		{
			{
				StringBuilder toDoResponse = new StringBuilder("");

				//using (TaskContext taskContext = new TaskContext()) {
				var messageParts = toDoRequest.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				if (messageParts.Length > 1) {
					string toDoCommand = messageParts[1];

					switch (toDoCommand) {
					case "clear": {
							using (TaskContext taskContext = new TaskContext()) {
								//IEnumerable<TaskModel> taskModels = taskContext.Tasks; 
								foreach (var task in taskContext.ToDos) {
									taskContext.ToDos.Remove(task);
								}
								//taskContext.Tasks.RemoveRange(taskContext.Tasks); 
								taskContext.SaveChanges();
								toDoResponse.Append("Список заданий очищен.");
							}
						}
						break;
					//сделать через "список"
					case "list": {
							using (TaskContext taskContext = new TaskContext()) {
								foreach (var task in taskContext.ToDos) {
									toDoResponse.Append(task.CreationtionDate);
									toDoResponse.Append(" ");
									toDoResponse.Append(task.Description);
									toDoResponse.Append("\n");
								}

								if (taskContext.ToDos.Count() == 0)
									toDoResponse.Append("у вас нет напоминаний");
							}
						}
						break;
					//сделать через "напомни ..."
					case "add": {
							if (messageParts.Length > 2) {
								string toDoText = toDoRequest.Remove(0, 9);

								if (ToDoManager.AddToDo(toDoText))
									toDoResponse.Append($"({toDoText}) добавлено в список дел.");
							}
						}
						break;
					}
				} else
					return "/task clear - очистить список задач\n/task list - вывести список напоминаний\n/task add {что напомнить}- добавить напоминание";
				//}

				return toDoResponse.ToString();
			}
		}

		public string BuiltCurrencyResponse(string taskRequest)
		{
			var messageParts = taskRequest.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			//var kindCurrency = messageParts.Length == 1 ? "USDBYR" : messageParts.Skip(1).First();

			//WebUtility.UrlEncode(kindCurrency);

			//string url =
			//	string.Format(
			//		"https://query.yahooapis.com/v1/public/yql?q=select+*+from+yahoo.finance.xchange+where+pair+=+%22{0}%22&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=",
			//		kindCurrency);			
			string url = "http://www.apilayer.net/api/live?access_key=d02cf1bc79ea22f8f6cd4fdbb5486a01&currencies=BYN&format=1";

			WebRequest request = WebRequest.Create(url);
			WebResponse response = request.GetResponse();

			using (var streamReader = new StreamReader(response.GetResponseStream())) {
				string responseString = streamReader.ReadToEnd();

				Console.WriteLine($"response string: {responseString}");
				_log.Info($"date time - {DateTime.Now}, response string: {responseString}");

				ApilayerCurrency currency = new ApilayerCurrency(responseString);

				Console.WriteLine($"Курс нового беларусского рубля к доллару {currency.USDBYN}");
				_log.Info($"date time - {DateTime.Now}, Курс нового беларусского рубля к доллару {currency.USDBYN}");

				var message = $"Курс нового беларусского рубля к доллару {currency.USDBYN}";
				return message;
			}
		}

		public string BuiltBitcoinCurrencyResponse(string taskRequest)
		{
			var messageParts = taskRequest.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			//var kindCurrency = messageParts.Length == 1 ? "USDBYR" : messageParts.Skip(1).First();

			//WebUtility.UrlEncode(kindCurrency);

			//string url =
			//	string.Format(
			//		"https://query.yahooapis.com/v1/public/yql?q=select+*+from+yahoo.finance.xchange+where+pair+=+%22{0}%22&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=",
			//		kindCurrency);			
			string url = "https://api.coinmarketcap.com/v1/ticker/bitcoin/";

			WebRequest request = WebRequest.Create(url);
			WebResponse response = request.GetResponse();

			using (var streamReader = new StreamReader(response.GetResponseStream())) {
				string responseString = streamReader.ReadToEnd();

				Console.WriteLine(responseString);
				_log.Info($"date time - {DateTime.Now}, response string: {responseString}");

				BitcoinCurrency currency = new BitcoinCurrency(responseString);

				Console.WriteLine($"Курс биткоина к доллару {currency.BitcoinUSD}");

				var message = $"Курс биткоина к доллару {currency.BitcoinUSD}";
				_log.Info($"date time - {DateTime.Now}, message {message}");

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
			} else
			if (taskRequest.Contains("какой сейчас курс биткоина?")) {
				return BuiltBitcoinCurrencyResponse(taskRequest);
			} else
				if (taskRequest.Contains("напомни мне ")) {
				ToDoManager.AddToDo(taskRequest.Remove(0, "напомни мне ".Length));
				return "хорошо напомню";
			} else
				if (taskRequest.Contains("список")) {
				return ToDoManager.ShowList();
			} else
			if (taskRequest.Contains("покажи кота")) {
				return ShowCat();
				//return "http://25.media.tumblr.com/tumblr_m2vycqKiHM1qc96lqo1_1280.jpg";
			} else
			if (taskRequest.Contains("покажи лог")) {
				var rootAppender = ((Hierarchy) LogManager.GetRepository())
										 .Root.Appenders.OfType<FileAppender>()
										 .FirstOrDefault();

				string filename = rootAppender != null ? rootAppender.File : string.Empty;

				return "хрен";
			} else 
			if (taskRequest.Contains("покажи")) {
				return ShowPictureFromGoogle(taskRequest.Remove(0, 6));
			} else {
				StringBuilder taskResponse = new StringBuilder("");

				using (BotContext botContext = new BotContext()) {
					string[] tags = taskRequest.ToLower().Split();
					string ansver = "";

					if (string.IsNullOrEmpty(ansver)) {
						taskResponse.Append("к такому я не был готов");
					} else {
						taskResponse.Append(ansver);
					}
				}

				return taskResponse.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>URL with random cat picture</returns>
		private string ShowCat()
		{
			StringBuilder result = new StringBuilder();
			WebRequest request = WebRequest.Create("http://thecatapi.com/api/images/get?format=xml&results_per_page=1&api_key=MjQ4ODIy");
			WebResponse response = request.GetResponse();

			using (var streamReader = new StreamReader(response.GetResponseStream())) {
				string responseString = streamReader.ReadToEnd();

				Console.WriteLine($"response string: {responseString}");
				_log.Info($"date time - {DateTime.Now}, response string: {responseString}");

				XmlDocument doc = new XmlDocument();
				doc.LoadXml(responseString);

				result.Append(doc.ChildNodes[1].FirstChild.FirstChild.FirstChild.FirstChild.InnerText);
			}

			return result.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>Random cat picture URL found by google</returns>
		private string ShowPictureFromGoogle(string requestText)
		{			
			try {
				using (IWebDriver driver = new ChromeDriver()) {
					driver.Navigate().GoToUrl("https://www.google.by/");
					IWebElement searchInput = driver.FindElement(By.XPath("//*[@id='lst-ib']"));
					searchInput.SendKeys(requestText);
					searchInput.SendKeys(Keys.Enter);

					IWebElement picturesTab = driver.FindElement(By.XPath("//*[@id='hdtb-msb-vis']/div[2]/a"));
					picturesTab.Click();

					driver.FindElement(By.XPath("//*[@id='rg_s']/div[1]/a/img")).Click(); // сликнуть первую картинку
					var result = driver.FindElement(By.XPath("//img[contains(@src,'http')]")).GetAttribute("src"); //взять урлу большого варианта

					return result;
				}
			} catch (Exception ex) {

			}

			return "";
		}
	}
}
