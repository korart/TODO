using Model.Domain;

namespace BussinesLogic
{
	public class TodoItemEventArgs
	{
		public TodoItem TodoItem { get; set; }
		public TodoItemEventArgs(TodoItem todoItem)
		{
			TodoItem = todoItem;
		}
	}
}
