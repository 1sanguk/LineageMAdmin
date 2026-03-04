using LineageMOps.Data;
using LineageMOps.Services;
using LineageMOps.Services.Sql;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

bool useMock = builder.Configuration.GetValue<bool>("UseMockData", true);

// MockDataStore is always registered — SqlMonitoringService uses it for live server statuses
builder.Services.AddSingleton(MockDataStore.Instance);

if (!useMock)
{
    builder.Services.AddDbContext<LineageMOpsDbContext>(opts =>
        opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IUserService, SqlUserService>();
    builder.Services.AddScoped<IGameDataService, SqlGameDataService>();
    builder.Services.AddScoped<IEventService, SqlEventService>();
    builder.Services.AddScoped<IMonitoringService, SqlMonitoringService>();
    builder.Services.AddScoped<IAdminLogService, SqlAdminLogService>();
}
else
{
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IGameDataService, GameDataService>();
    builder.Services.AddScoped<IEventService, EventService>();
    builder.Services.AddScoped<IMonitoringService, MonitoringService>();
    builder.Services.AddScoped<IAdminLogService, AdminLogService>();
}

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Dashboard/Index");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

// Initialize DB on first run (only in SQL mode)
if (!useMock)
{
    using var scope = app.Services.CreateScope();
    DbInitializer.Initialize(scope.ServiceProvider);
}

app.Run();
