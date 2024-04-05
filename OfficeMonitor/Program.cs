using CRUD.implementation;
using CRUD.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OfficeMonitor.DataBase;
using OfficeMonitor.Mapper;
using OfficeMonitor.Services;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<AppDbContext>();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddScoped<ActionRepo>();

builder.Services.AddScoped<ActionService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefautConnection"))
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

builder.Services.AddControllers()
   .AddJsonOptions(options =>
   {
       options.JsonSerializerOptions.PropertyNamingPolicy = null;
   });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Personal-Testing-System", Version = "v1" });

    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
        }
                });
    //c.EnableAnnotations();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
