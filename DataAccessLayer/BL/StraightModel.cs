using Model.DAL;
using Model.Domain;

namespace Model.BL
{
	public class StraightModel : IModel
	{
		public event EventHandler<TodoItemEventArgs>? UpdatedEvent;
		public event EventHandler<TodoItemEventArgs>? DeletedEvent;
		public event EventHandler<TodoItemEventArgs>? CreatedEvent;

		private IRepository<TodoItem> _repository;

		public StraightModel(IRepository<TodoItem> repository)
		{
			_repository = repository;
		}

		public List<TodoItem> ReadTodoItems()
		{
			return _repository.Read().OrderBy(o => o.Id).ToList();
		}

		public void CreateTodoItem(TodoItem TodoItem)
		{
			Validator.CreateValidation(TodoItem);
			try
			{
				_repository.Create(TodoItem);
				CreatedEvent?.Invoke(this, new TodoItemEventArgs(TodoItem));
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
				_repository.Delete(TodoItem.Id);
				DeletedEvent?.Invoke(this, new TodoItemEventArgs(TodoItem));
			}
			catch (RepositoryException ex)
			{
				throw new BussinesLogicException(ex.Message);
			}
		}

		public TodoItem ReadTodoItem(int id)
		{
			TodoItem? TodoItem;
			TodoItem = _repository.Read(id);
			// модель может возвращать null, но бизнес-логика нет
			if (TodoItem == null)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionItemNotFind") ?? "");
			}
			else
			{
				return TodoItem;
			}
		}

		public void UpdateTodoItem(TodoItem TodoItem)
		{
			TodoItem? previous = _repository.Read(TodoItem.Id);
			Validator.UpdateValidation(TodoItem, previous);
			try
			{
				_repository.Update(TodoItem);
				UpdatedEvent?.Invoke(this, new TodoItemEventArgs(TodoItem));
			}
			catch (RepositoryException ex)
			{
				throw new BussinesLogicException(ex.Message);
			}
		}

		public void SaveAll()
		{
			_repository.SaveAll(null);
		}
	}
}
