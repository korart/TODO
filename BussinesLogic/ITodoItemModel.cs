using Model.Domain;

namespace BussinesLogic
{
	public interface ITodoItemModel
	{
		event EventHandler<TodoItemEventArgs>? EventCreateTodoItem;
        event EventHandler<TodoItemEventArgs>? EventDeleteTodoItem;
		event EventHandler<TodoItemEventArgs>? EventUpdateTodoItem;

		void CreateTodoItem(TodoItem TodoItem);
        void DeleteTodoItem(TodoItem TodoItem);
        IList<TodoItem> ReadTodoItems();
		TodoItem? ReadTodoItem(int id);
		void UpdateTodoItem(TodoItem TodoItem);
	}
}
