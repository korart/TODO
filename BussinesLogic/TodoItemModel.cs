using Model.DAL;
using Model.Domain;

namespace BussinesLogic
{
	public class TodoItemModel(IRepository<TodoItem> repository) : ITodoItemModel
	{
		// Статусы:  Назначена (1), Выполняется (2), Приостановлена (3_, Завершена (4)

		public event EventHandler<TodoItemEventArgs>? EventCreateTodoItem;
		public event EventHandler<TodoItemEventArgs>? EventDeleteTodoItem;
		public event EventHandler<TodoItemEventArgs>? EventUpdateTodoItem;

		public void CreateTodoItem(TodoItem TodoItem)
		{
			try
			{
				repository.Create(TodoItem);
				EventCreateTodoItem?.Invoke(this, new TodoItemEventArgs(TodoItem));
			}
			catch (RepositoryException ex)
			{
				throw new BussinesLogicException(ex.Message);
			}
		}

		public void DeleteTodoItem(TodoItem TodoItem)
		{
			try
			{
				repository.Delete(TodoItem);
				EventDeleteTodoItem?.Invoke(this, new TodoItemEventArgs(TodoItem));
			}
			catch (RepositoryException ex)
			{
				throw new BussinesLogicException(ex.Message);
			}
		}

		public TodoItem? ReadTodoItem(int id)
		{
			return repository.Read(id);
		}

		public IList<TodoItem> ReadTodoItems()
		{
			return repository.Read().ToList();
		}

		public void UpdateTodoItem(TodoItem TodoItem)
		{
			TodoItem? previous = repository.Read(TodoItem.Id);
			if (previous == null)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BussinesLogicExceptionUpdateNotFindPrevious") ?? "");
			}
			// Статусы:  Назначена (1), Выполняется (2), Приостановлена (3), Завершена (4)
			// Проверка на то, что если новый статус = 4, то предыдущий не меньше 2 
			if (TodoItem.Status == 4 && previous.Status < 2)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BussinesLogicExceptionUpdateStatus") ?? "");
			}
			try
			{
				repository.Update(TodoItem);
				EventUpdateTodoItem?.Invoke(this, new TodoItemEventArgs(TodoItem));
			}
			catch (RepositoryException ex)
			{
				throw new BussinesLogicException(ex.Message);
			}
		}
	}
}
