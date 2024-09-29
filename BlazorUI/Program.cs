using BlazorBootstrap;
using BlazorUI.Services;
using Model.BL;
using Model.DAL;
using Model.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IRepository<TodoItem>, MSSQLRepository>();
builder.Services.AddSingleton<IModel, StraightModel>();
builder.Services.AddSingleton<TodoItemService>();
builder.Services.AddSingleton<GridFiltersTranslationService>();
builder.Services.AddSingleton<AppState>();
builder.Services.AddBlazorBootstrap();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
