using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model.BL;

namespace MAUIUI.MVVM.ViewModels
{
	public partial class TaskDetailViewModel : BaseViewModel, IQueryAttributable
	{
		private readonly IModel _model;
		private DetailViewState State { get; set; }

		public event EventHandler CancelEvent;


		[ObservableProperty]
		TodoItemDTO todo;

		[ObservableProperty]
		string pageTitle;

		[ObservableProperty]
		List<Status> statuses = Shared.Statuses;

		[ObservableProperty]
		bool canDelete = false;

		[RelayCommand]
		async Task Cancel()
		{
			CancelEvent?.Invoke(this, EventArgs.Empty);
			await Shell.Current.GoToAsync("..");
		}

		[RelayCommand]
		async Task DeleteItem()
		{
			bool confirm = await Application.Current!.MainPage!.DisplayAlert("Удалить задачу?", "Вы действительно хотите удалить задачу", "Да", "Отмена");
			if (confirm)
			{
				try
				{
					_model.DeleteTodoItem(Todo.GetBase());
					await Shell.Current.GoToAsync("..");
				}
				catch (ApplicationException ex)
				{
					await Application.Current!.MainPage!.DisplayAlert("Ошибка!", ex.Message, "OK");
				}
			}
		}

		[RelayCommand]
		async Task SaveItem()
		{
			if (State.Todo.Id == 0)
			{
				try
				{
					_model.CreateTodoItem(State.Todo.GetBase());
					await Shell.Current.GoToAsync("..");
				}
				catch (ApplicationException ex)
				{
					await Application.Current!.MainPage!.DisplayAlert("Ошибка!", ex.Message, "OK");
				}
			}
			else
			{
				try
				{
					_model.UpdateTodoItem(State.Todo.GetBase());
					await Shell.Current.GoToAsync("..");
				}
				catch (ApplicationException ex)
				{
					await Application.Current!.MainPage!.DisplayAlert("Ошибка!", ex.Message, "OK");
				}
			}
		}

		// Разбираем переданный State по свойствам
		public void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			State = (DetailViewState)query["State"]!;
			Todo = State.Todo;
			PageTitle = State.PageTitle;
			CanDelete = State.CanDelete;
		}

		public TaskDetailViewModel(IModel model)
		{
			_model = model;
		}

		
	}
}
