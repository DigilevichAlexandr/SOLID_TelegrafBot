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
		private static readonly log4net.ILog log =
			log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private static void Main(string[] args)
		{
			try {
				IRequestResolve requestResolver = new RequestResolver(log);
				Run(requestResolver).Wait();
			} catch (Exception ex) {

			}
		}

		private static async Task Run(IRequestResolve requestResolver)
		{
			//try {
			var bot = new Api("454883056:AAFWlNt8oyeQouMasGb9JW824YGGk2c6znA");

			var me = await bot.GetMe();

			Console.WriteLine("Hello my name is {0}", me.Username);
			log.Info($"Hello my name is {me.Username}");

			var offset = 0;

			while (true) {
				var updates = await bot.GetUpdates(offset);

				foreach (var update in updates) {
					if (update.Message?.Type == MessageType.TextMessage) {
						try {
							string responseText = requestResolver.GetResponceText(update.Message.Text.ToLower(), bot, update);
							var t = await bot.SendTextMessage(update.Message.Chat.Id, responseText);
							Console.WriteLine($"response text: {responseText}");
							log.Info($"response text: {responseText}");
						} catch (Exception ex) {
							Console.WriteLine(ex.Message);
							log.Error(ex.Message);
						}
					}

					offset = update.Id + 1;
				}

				await Task.Delay(1000);
			}
		}
		//catch(Exception ex) {

		//}
	}
}