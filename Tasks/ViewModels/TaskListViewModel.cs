using Model.BL;
using System.Collections.ObjectModel;
using System.Windows;

namespace Tasks.ViewModels
{
	internal class TaskListViewModel : BindableBase
	{
		private IModel _model;
		private IDialogService _dialogService;

		public ObservableCollection<TodoItemDTO> Tasks { get; private set; } =
			new ObservableCollection<TodoItemDTO>();

		private TodoItemDTO _selectedTask = null;
		public TodoItemDTO SelectedTask
		{
			get => _selectedTask;
			set
			{
				SetProperty<TodoItemDTO>(ref _selectedTask, value);
			}
		}

		public TaskListViewModel(IModel model, IDialogService dialogService)
		{
			_model = model;
			_model.DeletedEvent += DeletedEventHandler;
			_model.UpdatedEvent += UpdatedEventHandler;
			_model.CreatedEvent += CreatedEventHandler;

			_dialogService = dialogService;

			LoadTasks(_model);
		}

		private void CreatedEventHandler(object? sender, TodoItemEventArgs e)
		{
			LoadTasks(_model);
		}

		private void UpdatedEventHandler(object? sender, TodoItemEventArgs e)
		{
			LoadTasks(_model);
		}

		private void DeletedEventHandler(object? sender, TodoItemEventArgs e)
		{
			LoadTasks(_model);
		}

		private void LoadTasks(IModel model)
		{
			Tasks.Clear();
			foreach (var item in model.ReadTodoItems())
			{
				Tasks.Add(new TodoItemDTO(item));
			}
		}

		private DelegateCommand _commandShowDetails;
		public DelegateCommand ShowDetails => _commandShowDetails ?? (_commandShowDetails = new DelegateCommand(ShowDetailsExecute));

		private DelegateCommand _commandAddNew;
		public DelegateCommand AddNew => _commandAddNew ?? (_commandAddNew = new DelegateCommand(AddNewExecute));

		private DelegateCommand _commandDelete;
		public DelegateCommand Delete => _commandDelete ?? (_commandDelete = new DelegateCommand(DeleteExecute));

		private DelegateCommand _commandClose;
		public DelegateCommand WindowCloseCommand => _commandClose ?? (_commandClose = new DelegateCommand(WindowCloseCommandExecute));

		private void WindowCloseCommandExecute()
		{
			_model.SaveAll();
		}

		private void ShowDetailsExecute()
		{
			if (_selectedTask == null)
			{
				string messageBoxText = "Для редактирования необходимо выбрать задачу из списка";
				string caption = "Список задач";
				MessageBoxButton button = MessageBoxButton.OK;
				MessageBoxImage icon = MessageBoxImage.Error;
				MessageBoxResult result;

				result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
			}
			else
			{
				var dialogParameters = new DialogParameters();
				dialogParameters.Add("task", _selectedTask.Clone());
				_dialogService.ShowDialog("TaskDetailView", dialogParameters);
			}
		}

		private void AddNewExecute()
		{
			var dialogParameters = new DialogParameters();
			dialogParameters.Add("task", new TodoItemDTO());
			_dialogService.ShowDialog("TaskDetailView", dialogParameters);
		}

		private void DeleteExecute()
		{
			if (_selectedTask == null)
			{
				string messageBoxText = "Для удаления необходимо выбрать задачу из списка";
				string caption = "Список задач";
				MessageBoxButton button = MessageBoxButton.OK;
				MessageBoxImage icon = MessageBoxImage.Error;
				MessageBoxResult result;

				result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
			}
			else
			{
				string messageBoxText = "Вы действительно хотите удалить задачу?";
				string caption = "Список задач";
				MessageBoxButton button = MessageBoxButton.YesNo;
				MessageBoxImage icon = MessageBoxImage.Question;
				MessageBoxResult result;

				result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);

				if (result == MessageBoxResult.Yes)
				{
					try
					{
						_model.DeleteTodoItem(_selectedTask.GetBase());
					}
					catch (ApplicationException ex)
					{
						string messageError = ex.Message;
						string captionError = "Ошибка";
						MessageBoxButton buttonError = MessageBoxButton.OK;
						MessageBoxImage iconError = MessageBoxImage.Error;
						MessageBoxResult resultError;
						resultError = MessageBox.Show(messageError, captionError, buttonError, iconError, MessageBoxResult.OK);
					}
				}
			}
		}
	}
}
