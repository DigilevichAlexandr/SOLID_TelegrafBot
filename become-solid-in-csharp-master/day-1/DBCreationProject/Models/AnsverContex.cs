using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCreationProject.Models
{
    class AnsverContex:DbContext
    {
        public AnsverContex()
            :base("DBConnection")
        { }

        public DbSet<Ansver> Ansvers { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
