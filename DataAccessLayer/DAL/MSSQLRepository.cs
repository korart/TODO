using Model.Domain;

namespace Model.DAL
{
	public class MSSQLRepository : IRepository<TodoItem>
	{
		private DatabaseContext db;
		//private static int autoId;
		public MSSQLRepository()
        {
            db = new DatabaseContext();
			//TodoItem? lastTodoItem = db.TodoItems.OrderByDescending(u => u.Id).FirstOrDefault();
			//autoId = lastTodoItem == null ? 1 : lastTodoItem.Id + 1;
        }
        public TodoItem Create(TodoItem item)
		{
			if (this.Read(item.Id) != null)
			{
				throw new RepositoryException(Strings.ResourceManager.GetString("RepositoryExceptionCreate") ?? "");

			}
			else
			{
				//item.Id = autoId++;
				db.TodoItems.Add(item);
				db.SaveChanges();
				return item;
			}
		}

		public void Delete(int id)
		{
			var todoItem = db.TodoItems.FirstOrDefault(x => x.Id == id);
			if (todoItem != null)
			{
				db.TodoItems.Remove(todoItem);
				db.SaveChanges();
			}
			else
			{
				throw new RepositoryException(Strings.ResourceManager.GetString("RepositoryExceptionDelete") ?? "");
			}
		}

		public IEnumerable<TodoItem> Read()
		{
			return db.TodoItems.ToList();
		}

		public TodoItem? Read(int id)
		{
			return db.TodoItems.FirstOrDefault(x => x.Id == id);
		}

		public void SaveAll(IEnumerable<TodoItem>? items)
		{
			db.SaveChanges();
		}

		public void Update(TodoItem item)
		{
			var todoItem = db.TodoItems.FirstOrDefault(x => x.Id == item.Id);
			if (todoItem != null)
			{
				db.TodoItems.Remove(todoItem);
				db.TodoItems.Add(item);
				db.SaveChanges();
			}
			else
			{
				throw new RepositoryException(Strings.ResourceManager.GetString("RepositoryExceptionUpdate") ?? "");
			}
		}
	}
}
