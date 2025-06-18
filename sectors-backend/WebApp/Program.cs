using System.Threading.RateLimiting;
using BLL;
using DAL;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


DotNetEnv.Env.Load("../.env");
var host = Environment.GetEnvironmentVariable("HOST");
var port = Environment.GetEnvironmentVariable("PORT");
var user = Environment.GetEnvironmentVariable("USER");
var db = Environment.GetEnvironmentVariable("DB");
var dbKey = Environment.GetEnvironmentVariable("DBKEY");

var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={dbKey};Pooling=true;Minimum Pool Size=0;Maximum Pool Size=100;SearchPath=sectorstask;";
var frontendUrl = Environment.GetEnvironmentVariable("FRONTENDURL");

builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.CommandTimeout(60);
        npgsqlOptions.EnableRetryOnFailure(3);
    }), poolSize: 500);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policyBuilder =>
    {
        policyBuilder
            .WithOrigins(frontendUrl ?? "")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
    options.DefaultPolicyName = "Frontend";
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 10;
        limiterOptions.Window = TimeSpan.FromSeconds(1);
        limiterOptions.QueueLimit = 2;
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

builder.Services.AddScoped<PersonService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "sectors-task", Version = "v1" });
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors("Frontend");
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "sectors-task");
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseSession(); 


app.UseAuthorization();

app.MapStaticAssets();

app.UseRateLimiter();
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();