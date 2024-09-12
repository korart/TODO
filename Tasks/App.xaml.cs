using Model.BL;
using Model.DAL;
using Model.Domain;
using System.Windows;
using Tasks.ViewModels;
using Tasks.Views;


namespace Tasks
{
	public partial class App : PrismApplication
	{
		protected override Window CreateShell()
		{
			return Container.Resolve<Views.TaskListView>();
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterSingleton<IRepository<TodoItem>, JsonRepository<TodoItem>>();
			containerRegistry.RegisterSingleton<IModel, CachedModel>();
			containerRegistry.RegisterDialog<TaskDetailView, TaskDetailViewModel>();
		}
	}

}
