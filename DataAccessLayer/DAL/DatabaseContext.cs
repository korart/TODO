using Microsoft.EntityFrameworkCore;
using Model.Domain;

namespace Model.DAL
{
	internal class DatabaseContext : DbContext
	{
		public DbSet<TodoItem> TodoItems { get; set; } = null!;

		public DatabaseContext()
		{
			Database.EnsureDeleted();
			if (Database.EnsureCreated())
			{
				var todoItems = new TodoItem[]
				{
					new TodoItem
					{
						Title = "Task #1",
						Description = "Description for Task #1",
						Executors = "Alex Johnson",
						Status = 1,
						PlannedDuration = 10,
						Start = new DateTime(2024, 8, 20),
						Finish = null,
					},
					new TodoItem
					{
						Title = "Task #2",
						Description = "Description for Task #2",
						Executors = "Mary Brown",
						Status = 2,
						PlannedDuration = 10,
						Start = new DateTime(2024, 8, 21),
						Finish = null,
					},
					new TodoItem
					{
						Title = "Task #3",
						Description = "Description for Task #3",
						Executors = "Nikolas Smith",
						Status = 3,
						PlannedDuration = 10,
						Start = new DateTime(2024, 8, 22),
						Finish = null,
					},
					new TodoItem
					{
						Title = "Task #4",
						Description = "Description for Task #4",
						Executors = "Andrew Robinson",
						Status = 4,
						PlannedDuration = 10,
						Start = new DateTime(2024, 8, 23),
						Finish = DateTime.Now,
					},
					new TodoItem
					{
						Title = "Task #5",
						Description = "Description for Task #5",
						Executors = "Elizabeth Simpson",
						Status = 2,
						PlannedDuration = 10,
						Start = new DateTime(2024, 8, 24),
						Finish = null,
					},
					new TodoItem
					{
						Title = "Task #6",
						Description = "Description for Task #6",
						Executors = "Lu Chan",
						Status = 2,
						PlannedDuration = 10,
						Start = new DateTime(2024, 8, 25),
						Finish = null,
					},
					new TodoItem
					{
						Title = "Task #7",
						Description = "Description for Task #7",
						Executors = "Tadeush Kovalski",
						Status = 3,
						PlannedDuration = 10,
						Start = new DateTime(2024, 8, 26),
						Finish = null,
					},
					new TodoItem
					{
						Title = "Task #8",
						Description = "Description for Task #8",
						Executors = "Barbara N. Monterro",
						Status = 3,
						PlannedDuration = 10,
						Start = new DateTime(2024, 8, 27),
						Finish = null,
					},
					new TodoItem
					{
						Title = "Task #9",
						Description = "Description for Task #9",
						Executors = "Alex Johnson",
						Status = 4,
						PlannedDuration = 10,
						Start = new DateTime(2024, 8, 28),
						Finish = new DateTime(2024, 9, 15),
					},
					new TodoItem
					{
						Title = "Task #10",
						Description = "Description for Task #10",
						Executors = "Barbara N. Monterro",
						Status = 4,
						PlannedDuration = 10,
						Start = new DateTime(2024, 8, 29),
						Finish = new DateTime(2024, 9, 21),
					},
				};
				TodoItems.AddRange(todoItems);
				SaveChanges();
			}
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=tododb;Trusted_Connection=True;");
		}
	}
}
