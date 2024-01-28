using Carter;
using EazyScenes.Web;
using EazyScenes.Web.Middleware;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Settings
var appSettings = builder.Configuration
                         .GetSection("WebSettings")
                         .Get<AppSettings>();
builder.Services
       .AddOptions<AppSettings>()
       .BindConfiguration("WebSettings")
       .ValidateDataAnnotations()
       .ValidateOnStart();


builder.Services.AddApplicationInsightsTelemetry();

// Add services to the container.
builder.Services.AddCors(options =>
                         {
                             options.AddPolicy("CorsPolicy", p =>
                                                             {
                                                                 p.AllowAnyMethod()
                                                                  .AllowAnyHeader()
                                                                  .AllowCredentials()
                                                                  .WithOrigins(appSettings.ClientUrls);
                                                             });
                         });
builder.Services.AddDataContext(builder.Configuration);

// needed with minimal apis using Authorization used to use .AddControllersWithViews()
builder.Services.AddAuthorizationBuilder()
       .AddPolicy("User", policy => policy
                              .RequireRole("User"))
       .AddPolicy("Admin", policy => policy
                               .RequireRole("Admin"));

builder.Services.AddOpenIddictServer(builder.Environment);

builder.Services.AddUserIdentity();

builder.Services.AddAuthentication(o =>
                                   {
                                       o.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                       o.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                       o.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                       
                                   });

builder.Services.AddDbQueries();

builder.Services.AddFluentValidators();
builder.Services.AddMediatRServices();
builder.Services.AddCarter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandling>();

app.MapCarter(); // Maps all Minimal Api Endpoints that inherit ICarterModule/CarterModule

app.Run();
