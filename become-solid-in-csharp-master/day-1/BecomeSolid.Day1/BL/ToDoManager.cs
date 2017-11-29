using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BecomeSolid.Day1.DAL;
using BecomeSolid.Day1.Models;

namespace BecomeSolid.Day1.BL
{
	public static class ToDoManager
	{
		public static bool AddToDo(string toDoText)
		{
			if (!string.IsNullOrEmpty(toDoText)) {
				using (TaskContext toDoContext = new TaskContext()) {
					toDoContext.ToDos.Add(new ToDo() { CreationtionDate = DateTime.Now, Description = toDoText });
					toDoContext.SaveChanges();
					return true;
				}
			} else
				return false;
		}

		public static string ShowList()
		{
			StringBuilder result =new StringBuilder();

			using (TaskContext taskContext = new TaskContext()) {
				foreach (var task in taskContext.ToDos) {
					result.Append(task.CreationtionDate);
					result.Append(" просили напомнить вам ");
					result.Append(task.Description);
					result.Append("\n");
				}

				if (taskContext.ToDos.Count() == 0)
					return "у вас нет напоминаний";
			}

			return result.ToString();
		}
	}
}

