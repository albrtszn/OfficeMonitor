using CRUD.implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OfficeMonitor.Mapper;
using OfficeMonitor.Services;
using OfficeMonitor.Services.MasterService;
using DataBase.Repository;
using OfficeMonitor.ErrorHandler;
using OfficeMonitor.MiddleWares.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.Options;

//todo 1) create BdContextConfiguration
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
builder.Services.AddScoped<ClaimRoleRepo>();
builder.Services.AddScoped<TokenEmployeeRepo>();
builder.Services.AddScoped<TokenManagerRepo>();
builder.Services.AddScoped<TokenAdminRepo>();
builder.Services.AddScoped<TokenCompanyRepo>();

builder.Services.AddScoped<JwtProvider>();
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
builder.Services.AddScoped<ClaimRoleService>();
builder.Services.AddScoped<TokenEmployeeService>();
builder.Services.AddScoped<TokenManagerService>();
builder.Services.AddScoped<TokenAdminService>();
builder.Services.AddScoped<TokenCompanyService>();
builder.Services.AddScoped<MasterService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefautConnection"))
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = false,
            // будет ли валидироваться потребитель токена
            ValidateAudience = false,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            // todo secret key
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ssseeecccrrreeettt_kkkeeeyyy_12345")),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["cookie#1"];
                return Task.CompletedTask;
            }
        };
    });

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

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    /*var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);*/

    c.EnableAnnotations();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandler>();

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

app.UseCookiePolicy(new CookiePolicyOptions
{
    //  todo cookie polkicy
    //MinimumSameSitePolicy = SameSiteMode.Strict,
    //HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    //Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
