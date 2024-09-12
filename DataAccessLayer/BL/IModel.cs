using Model.Domain;

namespace Model.BL
{
	public interface IModel
	{
		event EventHandler<TodoItemEventArgs>? CreatedEvent;
		event EventHandler<TodoItemEventArgs>? DeletedEvent;
		event EventHandler<TodoItemEventArgs>? UpdatedEvent;

		void CreateTodoItem(TodoItem TodoItem);
		void DeleteTodoItem(TodoItem TodoItem);
		TodoItem ReadTodoItem(int id);
		List<TodoItem> ReadTodoItems();
		void UpdateTodoItem(TodoItem TodoItem);
		void SaveAll();
	}
}