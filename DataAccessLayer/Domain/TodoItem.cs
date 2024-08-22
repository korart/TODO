namespace Model.Domain
{
	public class TodoItem : IDomainObject
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Executors { get; set; }
		public int Status { get; set; }
		public int PlannedDuration { get; set; }
		public int ActualDuration { get; set; }
		public DateTime? Start { get; set; }
		public DateTime? Finish { get; set; }

		public TodoItem Clone()
		{
			return (TodoItem)MemberwiseClone();
		}
	}
}
