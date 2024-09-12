using Model.Domain;
using System.Diagnostics;

namespace Tasks.ViewModels
{
	public class TodoItemDTO : BindableBase
	{
		private string _title;
		private string? _description;
		private string? _executors;
		private Status _status;
		private int _plannedDuration;
		private DateTime _start;
		private DateTime? _finish;

		public int Id { get; init; }
		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}
		public string? Description
		{
			get => _description;
			set => SetProperty(ref _description, value);
		}
		public string? Executors
		{
			get => _executors;
			set => SetProperty(ref _executors, value);
		}
		public Status Status
		{
			get => _status;
			set => SetProperty(ref _status, value);
		}
		public int PlannedDuration
		{
			get => _plannedDuration;
			set => SetProperty(ref _plannedDuration, value);
		}
		public int ActualDuration { get; init; }
		public DateTime Start
		{
			get => _start;
			set => SetProperty(ref _start, value);
		}
		public DateTime? Finish
		{
			get => _finish;
			set => SetProperty(ref _finish, value);
		}

		public TodoItemDTO()
		{
			this.Start = DateTime.Now;
		}

		public TodoItemDTO(TodoItem item)
		{
			this.Id = item.Id;
			this.Title = item.Title!;
			this.Description = item.Description;
			this.PlannedDuration = item.PlannedDuration;
			this.ActualDuration = item.ActualDuration;
			this.Start = (DateTime)item.Start!;
			this.Finish = item.Finish;
			this.Executors = String.IsNullOrEmpty(item.Executors?.Trim()) ? null : item.Executors.Trim();

			this.Status = item.Status switch
			{
				1 => new Status { Id = 1, StatusName = "Назначена" },
				2 => new Status { Id = 2, StatusName = "Выполняется" },
				3 => new Status { Id = 3, StatusName = "Приостановлена" },
				4 => new Status { Id = 4, StatusName = "Завершена" },
				_ => throw new UnreachableException()
			};
		}

		public TodoItem GetBase()
		{
			TodoItem item = new TodoItem();
			item.Id = this.Id;
			item.Title = this.Title!;
			item.Description = this.Description;
			item.PlannedDuration = this.PlannedDuration;
			item.Start = this.Start;
			item.Finish = this.Finish;
			item.Executors = this.Executors;
			item.Status = this.Status == null ? 0 : this.Status.Id;
			return item;
		}

		public TodoItemDTO Clone()
		{
			TodoItemDTO result = (TodoItemDTO)this.MemberwiseClone();
			result.Status = new Status { Id = this.Status.Id, StatusName = this.Status.StatusName };
			return result;
		}
	}
}
