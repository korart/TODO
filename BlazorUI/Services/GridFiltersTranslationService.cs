using BlazorBootstrap;

namespace BlazorUI.Services
{
	public class GridFiltersTranslationService
	{
		public async Task<IEnumerable<FilterOperatorInfo>> GridFiltersTranslationProvider()
		{
			var filtersTranslation = new List<FilterOperatorInfo>();

			// number/date/boolean
			filtersTranslation.Add(new("=", "Равно", FilterOperator.Equals));
			filtersTranslation.Add(new("!=", "Не равно", FilterOperator.NotEquals));
			// number/date
			filtersTranslation.Add(new("<", "Меньше", FilterOperator.LessThan));
			filtersTranslation.Add(new("<=", "Меньше или равно", FilterOperator.LessThanOrEquals));
			filtersTranslation.Add(new(">", "Больше", FilterOperator.GreaterThan));
			filtersTranslation.Add(new(">=", "Больше или равно", FilterOperator.GreaterThanOrEquals));
			// string
			filtersTranslation.Add(new("*a*", "Содержит", FilterOperator.Contains));
			filtersTranslation.Add(new("a**", "Начинается с", FilterOperator.StartsWith));
			filtersTranslation.Add(new("**a", "Заканчиватеся на", FilterOperator.EndsWith));
			filtersTranslation.Add(new("=", "Равно", FilterOperator.Equals));
			// common
			filtersTranslation.Add(new("x", "Очистить", FilterOperator.Clear));

			return await Task.FromResult(filtersTranslation);
		}
	}
}
