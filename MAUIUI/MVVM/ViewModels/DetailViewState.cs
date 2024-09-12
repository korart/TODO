namespace MAUIUI.MVVM.ViewModels
{
	public partial class DetailViewState
	{
		public TodoItemDTO Todo { get; set; }

		public string PageTitle { get; set; }

		public bool CanDelete { get; set; }

        public DetailViewState(TodoItemDTO todo, string pageTitle, bool canDelete)
        {
            Todo = todo;
			PageTitle = pageTitle;
			CanDelete = canDelete;
        }
    }
}
