using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Tasks.Converters
{
	internal class StatusColorConverter : IValueConverter
	{
		public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//Назначена(1), Выполняется(2), Приостановлена(3), Завершена(4)
			return ((int)value) switch
			{
				1 => new SolidColorBrush((Color)Application.Current.Resources["AssignedColor"]),
				2 => new SolidColorBrush((Color)Application.Current.Resources["InProgressColor"]),
				3 => new SolidColorBrush((Color)Application.Current.Resources["SuspendedColor"]),
				4 => new SolidColorBrush((Color)Application.Current.Resources["CompletedColor"]),
				_ => new SolidColorBrush((Color)Application.Current.Resources["NotAssignedColor"]),
			};
		}

		public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
