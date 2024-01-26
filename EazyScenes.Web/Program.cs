using Carter;
using EazyScenes.Web;
using EazyScenes.Web.Middleware;

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


builder.Services.AddAuthentication(o => {})
       .AddCookie(o => {});


builder.Services.AddMediatRServices();
builder.Services.AddCarter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("CorsPolicy");

//app.UseAuthentication(); // put back after openiddict setup
//app.UseAuthorization();

app.UseMiddleware<ExceptionHandling>();

app.MapCarter(); // Maps all Minimal Api Endpoints that inherit ICarterModule/CarterModule

app.Run();
