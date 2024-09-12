using Model.DAL;
using Model.Domain;

namespace Model.BL
{
	public class CachedModel : IModel
	{
		public event EventHandler<TodoItemEventArgs>? UpdatedEvent;
		public event EventHandler<TodoItemEventArgs>? DeletedEvent;
		public event EventHandler<TodoItemEventArgs>? CreatedEvent;

		private List<TodoItem> _todoItems;
		private IRepository<TodoItem> _repository;

		public CachedModel(IRepository<TodoItem> repository)
		{
			_repository = repository;
			_todoItems = _repository.Read().OrderBy(o => o.Id).ToList();
		}

		public List<TodoItem> ReadTodoItems()
		{
			return _todoItems.OrderBy(o => o.Id).ToList();
		}

		public TodoItem ReadTodoItem(int id)
		{
			TodoItem? todoItem = _todoItems.FindById(id);
			if (todoItem == null)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionItemNotFind") ?? "");
			}
			else
			{
				return todoItem;
			}
		}

		public void CreateTodoItem(TodoItem todoItem)
		{
			Validator.CreateValidation(todoItem);
			try
			{
				_todoItems.Add(_repository.Create(todoItem));
				CreatedEvent?.Invoke(this, new TodoItemEventArgs(todoItem));
			}
			catch (RepositoryException ex)
			{
				throw new BussinesLogicException(ex.Message);
			}

		}

		public void DeleteTodoItem(TodoItem todoItem)
		{
			TodoItem? todoItemForDelete = _todoItems.FindById(todoItem.Id);
			if (_todoItems.Remove(todoItemForDelete))
			{
				DeletedEvent?.Invoke(this, new TodoItemEventArgs(todoItem));
			}
		}

		public void UpdateTodoItem(TodoItem todoItem)
		{
			TodoItem? previous = _todoItems.FindById(todoItem.Id);
			Validator.UpdateValidation(todoItem, previous);
			_todoItems.Remove(previous);
			_todoItems.Add(todoItem);
			UpdatedEvent?.Invoke(this, new TodoItemEventArgs(todoItem));
		}

		public void SaveAll()
		{
			_repository.SaveAll(_todoItems);
		}
	}

	public static class ListExtensions
	{
		public static TodoItem FindById(this List<TodoItem> todoItems, int id)
		{
			foreach (TodoItem item in todoItems)
			{
				if (item.Id == id)
				{
					return item;
				}
			}
			return null;
		}
	}
}
