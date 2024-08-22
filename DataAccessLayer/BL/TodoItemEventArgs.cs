using Model.Domain;

namespace Model.BL
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
