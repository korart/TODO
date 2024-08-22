using Model.Domain;
using Model.DAL;
using Model.BL;

namespace TestApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			IRepository<TodoItem> repo = new JsonRepository<TodoItem>();
			ITodoItemModel model = new TodoItemModel(repo);
			TodoItem item;

			//item = new TodoItem() { Id = 1, Title = "First Task", Description = "qqqq", Status = 1, Executors = "Admin" };
			//model.CreateTodoItem(item);

			//item = new TodoItem() { Id = 3, Title = "Third Task", Description = "qqqq", Status = 3 };
			//model.CreateTodoItem(item);

			item = new TodoItem() { Id = 2, Title = "Second Task", Description = "zzzz", Status = 2, Executors = "Вася, Петя" };
			model.CreateTodoItem(item);

			PrintTasks(model);

			model.DeleteTodoItem(item);

			item = model.ReadTodoItem(1);
			item.Status = 2;
			model.UpdateTodoItem(item);
			item.Status = 4;
			model.UpdateTodoItem(item);

			PrintTasks(model);

			item = model.ReadTodoItem(3);
			item.Executors = "Коля, Саша";
			item.Status = 1;
			model.UpdateTodoItem(item);

			PrintTasks(model);

			item = model.ReadTodoItem(3);
			item.Executors = "Ваня, Петя";
			model.UpdateTodoItem(item);

			PrintTasks(model);

			Console.WriteLine();
			try
			{
				item = new TodoItem() { Id = 3, Title = "First Task Edited", Description = "zzzz", Status = 3, Start = DateTime.Now };
				model.CreateTodoItem(item);
			}
			catch (BussinesLogicException ex)
			{
				Console.WriteLine(ex.Message);
			}

			try
			{
				item = new TodoItem() { Id = 100, Title = "First Task Edited", Description = "zzzz", Status = 3, Start = DateTime.Now };
				model.DeleteTodoItem(item);
			}
			catch (BussinesLogicException ex)
			{
				Console.WriteLine(ex.Message);
			}

			try
			{
				item = model.ReadTodoItem(3);
				item.Status = 4;
				model.UpdateTodoItem(item);
			}
			catch (BussinesLogicException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		static void PrintTasks(ITodoItemModel model)
		{
			Console.WriteLine();
			foreach (var i in model.ReadTodoItems())
			{
				Console.WriteLine("Id: {0}, Title: {1}, Description: {2}, Status: {3}, Start Date: {4}, Executors: {5}, Finish Date: {6}", i.Id, i.Title, i.Description, i.Status, i.Start, i.Executors, i.Finish);
			}
		}
	}
}
