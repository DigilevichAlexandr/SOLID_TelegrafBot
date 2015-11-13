using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BecomeSolid.Day1.Models;

namespace BecomeSolid.Day1.DAL
{
    class BotContext: DbContext
    {
        public BotContext()
            :base("DBConnectionAI")
        { }

        public DbSet<Ansver> Ansvers { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
