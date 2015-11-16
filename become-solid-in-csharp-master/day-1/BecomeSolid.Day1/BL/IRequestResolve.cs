using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BecomeSolid.Day1.BL
{
    interface IRequestResolve
    {
        string GetResponceText(string request, Api bot, Update update);
    }
}
