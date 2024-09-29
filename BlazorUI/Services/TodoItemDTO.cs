using Model.Domain;
using System.Diagnostics;

namespace BlazorUI.Services
{
	public class TodoItemDTO
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Executors { get; set; }
		public string Status { get; set; }
		public int PlannedDuration { get; set; }
		public int ActualDuration { get; set; }
		
		public DateOnly? Start { get; set; }
		public DateOnly? Finish { get; set; }

		public TodoItemDTO()
		{
			this.Start = DateOnly.FromDateTime(DateTime.Now);
		}

		public TodoItemDTO(TodoItem item)
		{
			this.Id = item.Id;
			this.Title = item.Title!;
			this.Description = item.Description;
			this.PlannedDuration = item.PlannedDuration;
			this.ActualDuration = item.ActualDuration;
			this.Start = DateOnly.FromDateTime((DateTime)item.Start!);
			this.Finish = item.Finish != null ? DateOnly.FromDateTime((DateTime)item.Finish) : null;
			this.Executors = String.IsNullOrEmpty(item.Executors?.Trim()) ? null : item.Executors.Trim();

			this.Status = item.Status switch
			{
				1 => "Назначена",
				2 => "Выполняется",
				3 => "Приостановлена",
				4 => "Завершена",
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
			item.Start = this.Start!.Value.ToDateTime(TimeOnly.Parse("00:00:00"));
			item.Finish = this.Finish != null ? this.Finish!.Value.ToDateTime(TimeOnly.Parse("00:00:00")) : null;
			item.Executors = this.Executors;
			item.Status = this.Status switch
			{
				"Назначена" => 1,
				"Выполняется" => 2,
				"Приостановлена" => 3,
				"Завершена" => 4,
				_ => throw new UnreachableException()
			};
			return item;
		}

		public TodoItemDTO Clone()
		{
			TodoItemDTO result = (TodoItemDTO)this.MemberwiseClone();
			return result;
		}
	}
}
