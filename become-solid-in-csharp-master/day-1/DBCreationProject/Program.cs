using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBCreationProject.Models;

namespace DBCreationProject
{
	class Program
	{
		static void Main(string[] args)
		{
			try {
				using (AnsverContex db = new AnsverContex()) {
					// создаем два объекта User 
					Answer a1 = new Answer { Value = "привет" };
					Answer a2 = new Answer { Value = "меня зовут Тимоша" };

					Question q1 = new Question { Value = "привет" };
					Question q2 = new Question { Value = "как тебя зовут" };

					// добавляем их в бд 
					db.Ansvers.Add(a1);
					db.Ansvers.Add(a2);
					db.Questions.Add(q1);
					db.Questions.Add(q2);
					db.SaveChanges();
					Console.WriteLine("Объекты успешно сохранены");

					// получаем объекты из бд и выводим на консоль 
					var ansvers = db.Ansvers;
					var questions = db.Questions;
					Console.WriteLine("Список объектов:");
					foreach (Answer ansver in ansvers) {
						Console.WriteLine("{0}.{1}", ansver.Id, ansver.Value);
					}
					foreach (Question question in questions) {
						Console.WriteLine("{0}.{1}", question.Id, question.Value);
					}
				}
				Console.Read();
			}
			catch(Exception ex) {

			}
		}
	}
}

