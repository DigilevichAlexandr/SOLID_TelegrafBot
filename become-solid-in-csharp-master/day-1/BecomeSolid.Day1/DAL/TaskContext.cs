using BecomeSolid.Day1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BecomeSolid.Day1.DAL
{
    class TaskContext:DbContext
    {
        public TaskContext()
            :base("DBConnection")
        { }

        public  DbSet<ToDo> ToDos { get; set; }
    }
}
