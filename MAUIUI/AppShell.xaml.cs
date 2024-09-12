using MAUIUI.MVVM.Views;

namespace MAUIUI
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();

			Routing.RegisterRoute(nameof(TaskDetail), typeof(TaskDetail));
		}
	}
}
