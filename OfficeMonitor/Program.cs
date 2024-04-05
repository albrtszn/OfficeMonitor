using CRUD.implementation;
using CRUD.interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeMonitor.DataBase;
using OfficeMonitor.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<AppDbContext>();

builder.Services.AddScoped<ActionRepo>();

builder.Services.AddScoped<ActionService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefautConnection"))
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
