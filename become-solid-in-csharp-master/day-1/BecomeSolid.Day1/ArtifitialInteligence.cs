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
            ansvers.Add("как дела?", "У меня рак.");
            ansvers.Add("я тебя люблю.", "Я тебя тоже.");
        }

        public string Ansver(string question)
        {
            string ansver;
            if (ansvers.TryGetValue(question,out ansver))
                return ansver;
            else
                return "Я не понял тебя?";
        }
    }
}
