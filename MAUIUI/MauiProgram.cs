using Model.BL;
using Model.DAL;
using Model.Domain;

namespace MAUIUI
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("FontAwesome.otf", "FontAwesome");
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				})
				.RegisterServices()
				.RegisterViewModels()
				.RegisterViews();
			return builder.Build();
		}

		public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
		{
			mauiAppBuilder.Services.AddSingleton<IRepository<TodoItem>, JsonRepository<TodoItem>>();
			mauiAppBuilder.Services.AddSingleton<IModel, CachedModel>();
			return mauiAppBuilder;
		}

		public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
		{
			mauiAppBuilder.Services.AddSingleton<MVVM.ViewModels.TaskListViewModel>();
			mauiAppBuilder.Services.AddSingleton<MVVM.ViewModels.TaskDetailViewModel>();
			return mauiAppBuilder;
		}

		public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
		{
			mauiAppBuilder.Services.AddSingleton(serviceProvider => new MVVM.Views.TaskList
			{
				BindingContext = serviceProvider.GetRequiredService<MVVM.ViewModels.TaskListViewModel>()
			});
			mauiAppBuilder.Services.AddTransient(serviceProvider => new MVVM.Views.TaskDetail
			{
				BindingContext = serviceProvider.GetRequiredService<MVVM.ViewModels.TaskDetailViewModel>()
			});
			return mauiAppBuilder;
		}
	}
}
