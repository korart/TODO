using CommunityToolkit.Mvvm.ComponentModel;
using Model.Domain;
using System.Diagnostics;

namespace MAUIUI.MVVM.ViewModels
{
	public partial class TodoItemDTO : ObservableObject
	{
		[ObservableProperty]
		int id;
		[ObservableProperty]
		string? title;
		[ObservableProperty]
		string? description;
		[ObservableProperty]
		string? executors;
		[ObservableProperty]
		Status? status;
		[ObservableProperty]
		int plannedDuration;
		[ObservableProperty]
		int actualDuration;
		[ObservableProperty]
		DateTime start;
		[ObservableProperty]
		DateTime? finish;
		[ObservableProperty]
		bool show = true;

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
				1 => Shared.Statuses.Where(s => s.Id == 1).First(),
				2 => Shared.Statuses.Where(s => s.Id == 2).First(),
				3 => Shared.Statuses.Where(s => s.Id == 3).First(),
				4 => Shared.Statuses.Where(s => s.Id == 4).First(),
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
			// В данном случае поверхностного копирования достаточно, т.к. статусы одни на всех
			//result.Status = Shared.Statuses.Where(s => s.Id == this.Status.Id).First();
			return result;
		}
	}
}
