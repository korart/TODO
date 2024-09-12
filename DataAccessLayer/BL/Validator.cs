using Model.Domain;

namespace Model.BL
{
	public static class Validator
	{
		public static void BasicValidation(TodoItem todoItem)
		{
			// Title - обязательное поле
			if (todoItem.Title == null)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionValidationCreateNoTitle") ?? "");
			}
			// Статус может быть только: Назначена (1), Выполняется (2), Приостановлена (3), Завершена (4)
			if (todoItem.Status < 1 || todoItem.Status > 4)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionValidationIncorrectStatus") ?? "");
			}
			// Если статус != 3, то Executors не может быть null или пусто
			if (todoItem.Status != 3 && string.IsNullOrEmpty(todoItem.Executors))
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionValidationNullExecutors") ?? "");
			}
		}
		public static void CreateValidation(TodoItem todoItem)
		{
			BasicValidation(todoItem);
			// Установка даты начала задачи
			todoItem.Start = DateTime.Now;
			// Статус новой задачи может быть только < 4
			if (todoItem.Status >= 4)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionValidationCreateStatusIs4") ?? "");
			}
		}

		public static void UpdateValidation(TodoItem current, TodoItem? previous)
		{
			BasicValidation(current);

			if (previous == null)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionUpdateNotFindPrevious") ?? "");
			}
			// Статусы:  Назначена (1), Выполняется (2), Приостановлена (3), Завершена (4)
			// Проверка на то, что если новый статус = 4, то предыдущий не меньше 2 
			if (current.Status == 4 && previous.Status < 2)
			{
				throw new BussinesLogicException(Strings.ResourceManager.GetString("BLExceptionUpdateStatus") ?? "");
			}
			if (current.Status == 4)
			{
				current.Finish = DateTime.Now;
			}
			else if (previous.Status == 4)
			{
				current.Finish = null;
			}
		}
	}
}
