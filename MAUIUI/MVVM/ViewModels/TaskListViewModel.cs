using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIUI.MVVM.Views;
using Model.BL;
using System.Collections.ObjectModel;

namespace MAUIUI.MVVM.ViewModels
{
	public partial class TaskListViewModel : BaseViewModel
	{
		private readonly IModel _model;
		private readonly IServiceProvider _serviceProvider;

		[ObservableProperty]
		TodoItemDTO? selectedItem;

		[ObservableProperty]
		bool isShowAssigned = true;
		[ObservableProperty]
		bool isShowInProgress = true;
		[ObservableProperty]
		bool isShowSuspended = true;
		[ObservableProperty]
		bool isShowCompleted = true;

		[ObservableProperty]
		ObservableCollection<TodoItemDTO> items = [];

		[RelayCommand]
		async Task AddItem()
		{
			var queryParameter = new Dictionary<string, object> { { "State", new DetailViewState(new TodoItemDTO(), "Новая задача", false) } };
			await Shell.Current.GoToAsync($"{nameof(TaskDetail)}", queryParameter);
		}

		// TODO: for the button in MainPage
		//[RelayCommand]
		//async Task EditItem() => await Shell.Current.GoToAsync(
		//	$"{nameof(TaskDetail)}",
		//	new Dictionary<string, object>
		//	{
		//		{
		//			"State", new DetailViewState(SelectedItem.Clone(), "Редактирование задачи", true)
		//		}
		//	});

		public TaskListViewModel(IModel model, IServiceProvider serviceProvider)
		{
			_model = model;
			_serviceProvider = serviceProvider;

			// Сохраняем данные при закрытии окна
			Application.Current!.MainPage!.Unloaded += SaveData;

			_model.CreatedEvent += UpdateList;
			_model.UpdatedEvent += UpdateList;
			_model.DeletedEvent += UpdateList;

			// Подписываемся на событие Cancel дочерней ViewModel чтобы "сбросить" выделенный элемент
			// При других событиях данные загружаются из модели и сброс происходит автоматически
			// По событию Cancel обращение к модели не происходит
			_serviceProvider.GetRequiredService<TaskDetailViewModel>().CancelEvent += UpdateListAfterCancel;

			LoadData();
		}

		private void UpdateListAfterCancel(object? sender, EventArgs e)
		{
			SelectedItem = null;
		}

		private void UpdateList(object? sender, TodoItemEventArgs e)
		{
			LoadData();
		}

		private void SaveData(object? sender, EventArgs e)
		{
			_model.SaveAll();
		}

		partial void OnSelectedItemChanging(TodoItemDTO? value)
		{
			if (value == null)
			{
				return;
			}
			var queryParameter = new Dictionary<string, object> { { "State", new DetailViewState(value.Clone(), "Редактирование задачи", true) } };
			
			Shell.Current.GoToAsync($"{nameof(TaskDetail)}", queryParameter);

			// Вариант перехода из web
			//MainThread.BeginInvokeOnMainThread(async () =>
			//{
			//	await Shell.Current.GoToAsync($"{nameof(TaskDetail)}", queryParameter);
			//});
		}

		partial void OnIsShowAssignedChanged(bool value)
		{
			foreach (var item in Items.Where(i => i.Status!.Id == 1))
			{
				item.Show = value;
			}
		}
		partial void OnIsShowInProgressChanged(bool value)
		{
			foreach (var item in Items.Where(i => i.Status!.Id == 2))
			{
				item.Show = value;
			}
		}
		partial void OnIsShowSuspendedChanged(bool value)
		{
			foreach (var item in Items.Where(i => i.Status!.Id == 3))
			{
				item.Show = value;
			}
		}
		partial void OnIsShowCompletedChanged(bool value)
		{
			foreach (var item in Items.Where(i => i.Status!.Id == 4))
			{
				item.Show = value;
			}
		}

		private void LoadData()
		{
			Items.Clear();
			foreach (var item in _model.ReadTodoItems())
			{
				Items.Add(new TodoItemDTO(item));
			}
			foreach (var item in Items)
			{
				switch (item.Status!.Id)
				{
					case 1: item.Show = IsShowAssigned; break;
					case 2: item.Show = IsShowInProgress; break;
					case 3: item.Show = IsShowSuspended; break;
					case 4: item.Show = IsShowCompleted; break;
				}
			}
		}
	}
}
