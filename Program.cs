using DigitalSignageSevice.Hubs;
using DigitalSignageSevice.MiddlewareExtensions;
using DigitalSignageSevice.MyBackgroundServices;
using DigitalSignageSevice.SubscribeTableDependencies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddHostedService<MyBackgroundService>();
builder.Services.AddSingleton<DashboardHub>();
builder.Services.AddSingleton<SubscribeDigitalSignageTableDependency>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "http://localhost:3001")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

var app = builder.Build();
var connectionString = app.Configuration.GetConnectionString("DefaultConnection");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("MyCorsPolicy");
app.UseAuthorization();
app.MapHub<DashboardHub>("/dashboardHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.UseDigitalSignalrTableDependency(connectionString);
app.Run();