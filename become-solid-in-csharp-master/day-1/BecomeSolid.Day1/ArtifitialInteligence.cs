using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BecomeSolid.Day1
{
    public class ArtifitialInteligence
    {
        Dictionary<string, string> ansvers = new Dictionary<string, string>();

        public ArtifitialInteligence()
        {
            ansvers.Add("привет", "Привет, мы знакомы?");
            ansvers.Add("как тебя зовут?", "Гоги, а тебя?");
            ansvers.Add("нет", "Гомогея ответ), ладно не обижайся, это я шучу типо так.");
            ansvers.Add("как дела?", "У меня рак.");
            ansvers.Add("сочуствую.", "Да пошел ты!");
            ansvers.Add("я тебя люблю.", "Я тебя тоже.");
            ansvers.Add("ты кто?", "Я тот, кто водил твою маму в ресторан. И нет я не твой папа, надеюсь.");
            //ansvers.Add("", "");
            //ansvers.Add("Привет", "");
            //ansvers.Add("Привет", "");
            //ansvers.Add("Привет", "");
            //ansvers.Add("Привет", "");
            //ansvers.Add("Привет", "");

        }

        public string Ansver(string question)
        {
            string ansver;
            if (ansvers.TryGetValue(question,out ansver))
                return ansver;
            else
                return "Ты случайно не наркоманьё?";
        }
    }
}
