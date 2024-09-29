using Model.BL;
using Model.Domain;


namespace BlazorUI.Services
{
    public class TodoItemService
    {
        private IServiceProvider serviceProvider;

        public TodoItemService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public Task<List<TodoItemDTO>> GetTodoItemsAsync()
        {
            List<TodoItemDTO> result = [];
            IModel model = serviceProvider.GetRequiredService<IModel>();

            foreach (TodoItem item in model.ReadTodoItems())
            {
                result.Add(new TodoItemDTO(item));
            }
            return Task.FromResult(result);
        }

        public Task DeleteTodoItemAsync(TodoItemDTO item)
        {
			IModel model = serviceProvider.GetRequiredService<IModel>();
			model.DeleteTodoItem(item.GetBase());
			return Task.CompletedTask;
		}

		public Task SaveTodoItemAsync(TodoItemDTO item)
		{
			IModel model = serviceProvider.GetRequiredService<IModel>();

            if (item.Id == 0 )
            {
				model.CreateTodoItem(item.GetBase());
			}
            else
            {
				model.UpdateTodoItem(item.GetBase());
			}
			return Task.CompletedTask;

		}

	}
}
