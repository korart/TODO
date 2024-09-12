namespace MAUIUI.MVVM.ViewModels
{
    public static class Shared
    {
		public static List<Status> Statuses = [
				new Status { Id = 1, StatusName = "Назначена" },
				new Status { Id = 2, StatusName = "Выполняется" },
				new Status { Id = 3, StatusName = "Приостановлена" },
				new Status { Id = 4, StatusName = "Завершена" },
				];
	}
}
