using System.Globalization;

namespace MAUIUI.Converters
{
	internal class StatusColorConverter : IValueConverter
	{
		public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//Назначена(1), Выполняется(2), Приостановлена(3), Завершена(4)
			int status = (int)value;
			Color color = status switch
			{
				1 => (Color)Application.Current!.Resources["AssignedColor"],
				2 => (Color)Application.Current!.Resources["InProgressColor"],
				3 => (Color)Application.Current!.Resources["SuspendedColor"],
				4 => (Color)Application.Current!.Resources["CompletedColor"],
				_ => (Color)Application.Current!.Resources["NotAssignedColor"],
			};
			return color;
		}

		public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
