
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.OpenApi.Models;
using MyWindowsService.HostedService;
using Serilog;


//using IHost host = Host.CreateDefaultBuilder(args)
//    .UseWindowsService()
//    .ConfigureServices(services =>
//    {
//        LoggerProviderOptions.RegisterProviderOptions<
//            EventLogSettings, EventLogLoggerProvider>(services);

//        services.AddControllers();
//        services.AddSwaggerGen(options =>
//        {
//            options.SwaggerDoc("v1", new OpenApiInfo
//            {
//                Version = "v1",
//                Title = "API标题",
//                Description = "API描述"
//            });
//        });
//        services.AddHostedService<SchedulerHostedService>();
//    })
//    .ConfigureLogging((context, logging) =>
//    {
//        // See: https://github.com/dotnet/runtime/issues/47303
//        logging.AddConfiguration(
//            context.Configuration.GetSection("Logging"));


//    })
//    .UseSerilog((hostingContext, services) =>
//    { 
//        services.WriteTo
//  .Console().
//  ReadFrom.Configuration(hostingContext.Configuration);
//    })
       
//    .Build();

//await host.RunAsync();


var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
};

 
var builder = WebApplication.CreateBuilder(args);
if (!string.IsNullOrWhiteSpace(options.ContentRootPath))
    Directory.SetCurrentDirectory(options.ContentRootPath);
 
var startupLogPath = $"{options.ContentRootPath??"."}/Logs/start-log.txt";
var directory = Path.GetDirectoryName(startupLogPath);
if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
{
    Directory.CreateDirectory(directory);
}
await File.WriteAllTextAsync(startupLogPath, null);
await File.AppendAllLinesAsync(startupLogPath, new[] {
    "ContentRootPath:"+options.ContentRootPath ?? "",
    "WebRootPath:"+ options.WebRootPath ?? "",
    "Environment.ContentRootPath:"+builder.Environment.ContentRootPath??"",
    "Environment.WebRootPath:"+builder.Environment.WebRootPath??""
   });
 
builder.Host.UseWindowsService();
  
// Add services to the container.
builder.Host.UseSerilog((context, configuration) =>
{ 
    configuration
    .WriteTo.Console()
    .ReadFrom.Configuration(context.Configuration)
    ; 


});
builder.Services.AddHostedService<SchedulerHostedService>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API标题",
        Description = "API描述"
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   
}
app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();
await File.AppendAllLinesAsync(startupLogPath, new[] { "service is starting......" });
app.Run();
