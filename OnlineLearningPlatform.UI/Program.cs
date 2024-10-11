using OnlineLearningPlatform.Application.Mappings;
using OnlineLearningPlatform.UI.Services.IServices;
using OnlineLearningPlatform.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using OnlineLearningPlatform.Infrastructure.Configurations;
using OnlineLearningPlatform.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// configure database
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// configure identity
builder.Services.AddIdentityConfiguration();


// configure lifetime for services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, TokenProvider>();

builder.Services.AddSignalR();

// configure automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// adding authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins(
                "https://cdn.jsdelivr.net",
                "https://code.jquery.com",
                "https://cdn.datatables.net",
                "https://cdnjs.cloudflare.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // If you're using cookies or authentication
        });
});



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

app.UseCors("AllowSpecificOrigins");


app.UseRouting();

app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "MyAreaInstructor",
    areaName: "Instructor",
    pattern: "Instructor/{controller=JobListing}/{action=Index}");

app.MapAreaControllerRoute(
    name: "MyAreaArchitect",
    areaName: "Architect",
    pattern: "Architect/{controller=JobListing}/{action=Index}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.SeedDatabaseAsync();

app.Run();
