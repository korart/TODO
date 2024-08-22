using Model.DAL;
using Model.Domain;

namespace Model.BL
{
	public class TodoItemModel(IRepository<TodoItem> repository) : ITodoItemModel
	{
		public event EventHandler<TodoItemEventArgs>? EventCreateTodoItem;
		public event EventHandler<TodoItemEventArgs>? EventDeleteTodoItem;
		public event EventHandler<TodoItemEventArgs>? EventUpdateTodoItem;

		public void CreateTodoItem(TodoItem TodoItem)
		{
			CreateValidation(TodoItem);
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

		// Возвращает клон объекта
		public TodoItem ReadTodoItem(int id)
		{
			TodoItem? TodoItem;
			TodoItem = repository.Read(id);
			// модель может возвращать null, но бизнес-логика нет
			if (TodoItem == null)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionItemNotFind") ?? "");
			}
			else
			{
				return TodoItem.Clone();
			}
		}

		public IList<TodoItem> ReadTodoItems()
		{
			return repository.Read().ToList();
		}

		public void UpdateTodoItem(TodoItem TodoItem)
		{
			UpdateValidation(TodoItem);
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

		private void CreateValidation(TodoItem todoItem)
		{
			BasicValidation(todoItem);
			// Установка даты начала задачи
			todoItem.Start = DateTime.Now;
			// Статус новой задачи может быть только < 4
			if (todoItem.Status == 4)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionValidationCreateStatusIs4") ?? "");
			}
		}

		private void UpdateValidation(TodoItem todoItem)
		{
			BasicValidation(todoItem);
			TodoItem? previous = repository.Read(todoItem.Id);
			if (previous == null)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionUpdateNotFindPrevious") ?? "");
			}
			// Статусы:  Назначена (1), Выполняется (2), Приостановлена (3), Завершена (4)
			// Проверка на то, что если новый статус = 4, то предыдущий не меньше 2 
			if (todoItem.Status == 4 && previous.Status < 2)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionUpdateStatus") ?? "");
			}
			if (todoItem.Status == 4)
			{
				todoItem.Finish = DateTime.Now;
			}
		}

		private void BasicValidation(TodoItem todoItem)
		{
			// Title - обязательное поле
			if (todoItem.Title == null)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionValidationCreateNoTitle") ?? "");
			}
			// Статус может быть только: Назначена (1), Выполняется (2), Приостановлена (3), Завершена (4)
			if (todoItem.Status < 1 || todoItem.Status > 4)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionValidationIncorrectStatus") ?? "");
			}
			// Если статус != 3, то Executors не может быть null или пусто
			if (todoItem.Status != 3 && string.IsNullOrEmpty(todoItem.Executors))
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionValidationNullExecutors") ?? "");
			}
		}
	}
}
