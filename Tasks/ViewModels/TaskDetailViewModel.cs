using Model.BL;
using System.Windows;

namespace Tasks.ViewModels
{
	internal class TaskDetailViewModel : BindableBase, IDialogAware
	{
		private TodoItemDTO _todoItem;
		private IModel _model;
		private bool isCanClose = true;

		public string Title { get; set; }

		public static List<Status> Statuses
		{
			get => [
				new Status { Id = 1, StatusName = "Назначена" },
				new Status { Id = 2, StatusName = "Выполняется" },
				new Status { Id = 3, StatusName = "Приостановлена" },
				new Status { Id = 4, StatusName = "Завершена" },
				];
		}

		public TodoItemDTO ToDo
		{
			get => _todoItem;
			set => SetProperty(ref _todoItem, value);
		}

		public DialogCloseListener RequestClose { get; }

		public TaskDetailViewModel(IModel model)
		{
			_model = model;
			Title = "Новая задача";
		}

		public virtual void OnDialogOpened(IDialogParameters parameters)
		{
			ToDo = parameters.GetValue<TodoItemDTO>("task");
			if (ToDo.Id == 0)
			{
				ToDo.Status = Statuses[2];
			}
			Title = ToDo.Id == 0 ? "Создание задачи" : "Редактирование задачи";
		}

		public bool CanCloseDialog()
		{
			return isCanClose;
		}

		public void OnDialogClosed()
		{

		}

		private DelegateCommand _commandCancel;
		public DelegateCommand CancelCommand => _commandCancel ?? (_commandCancel = new DelegateCommand(CancelCommandExecute));

		private void CancelCommandExecute()
		{
			isCanClose = true;
			RequestClose.Invoke();
		}

		private DelegateCommand _commandSave;
		public DelegateCommand SaveCommand => _commandSave ?? (_commandSave = new DelegateCommand(SaveCommandExecute));

		private void SaveCommandExecute()
		{
			ToDo.Title = ToDo.Title.Trim();
			ToDo.Description = ToDo.Description == null ? null : ToDo.Description?.Trim();
			ToDo.Executors = ToDo.Executors == null ? null : ToDo.Executors?.Trim();
			if (ToDo.Id == 0)
			{
				try
				{
					_model.CreateTodoItem(ToDo.GetBase());
					isCanClose = true;
				}
				catch (ApplicationException ex)
				{
					ShowMessage(ex.Message);
					isCanClose = false;
				}
			}
			else
			{
				try
				{
					_model.UpdateTodoItem(ToDo.GetBase());
					isCanClose = true;
				}
				catch (ApplicationException ex)
				{
					ShowMessage(ex.Message);
					isCanClose = false;
				}
			}
			RequestClose.Invoke();
		}

		private static MessageBoxResult ShowMessage(string text)
		{
			string message = text;
			string caption = "Ошибка";
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage icon = MessageBoxImage.Error;
			return MessageBox.Show(message, caption, button, icon, MessageBoxResult.OK);
		}
	}
}
