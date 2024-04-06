using CRUD.implementation;
using CRUD.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OfficeMonitor.DataBase;
using OfficeMonitor.Mapper;
using OfficeMonitor.Services;
using Newtonsoft.Json.Serialization;
using OfficeMonitor.Services.MasterService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<AppDbContext>();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddScoped<ActionRepo>();
builder.Services.AddScoped<AdminRepo>();
builder.Services.AddScoped<AppRepo>();
builder.Services.AddScoped<CompanyRepo>();
builder.Services.AddScoped<CustomerRequestRepo>();
builder.Services.AddScoped<DepartmentAppRepo>();
builder.Services.AddScoped<DepartmentManagerRepo>();
builder.Services.AddScoped<DepartmentRepo>();
builder.Services.AddScoped<EmployeeRepo>();
builder.Services.AddScoped<ManagerRepo>();
builder.Services.AddScoped<PlanRepo>();
builder.Services.AddScoped<ProfileRepo>();
builder.Services.AddScoped<TypeAppRepo>();
builder.Services.AddScoped<WorkTimeRepo>();

builder.Services.AddScoped<ActionService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<AppService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<CustomerRequestService>();
builder.Services.AddScoped<DepartmentAppService>();
builder.Services.AddScoped<DepartmentManagerService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<ManagerService>();
builder.Services.AddScoped<PlanService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<TypeAppService>();
builder.Services.AddScoped<WorkTimeService>();
builder.Services.AddScoped<MasterService>();

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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OfficeMonitor", Version = "v1" });

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
    c.EnableAnnotations();
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
