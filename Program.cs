using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using ScreenshotService.Models;
using ScreenshotService.Services;
using ScreenshotService.Middleware;
using Serilog;
using Prometheus;
using FluentValidation;
using FluentValidation.AspNetCore;
using Amazon;
using Amazon.S3;
using Amazon.Runtime;
using ScreenshotService.Helpers.Rendering;
using ScreenshotService.Helpers;
using ScreenshotService.Helpers.Viewports;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/screenshotServiceLog-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
        retainedFileCountLimit: 7
    )
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<S3Settings>(builder.Configuration.GetSection("S3"));
builder.Services.PostConfigure<S3Settings>(settings => settings.Validate());

builder.Services.AddSingleton<ScreenshotGenerator>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ScreenshotRequestValidator>();

builder.Services.AddSingleton<IScreenshotRenderer, PngRenderer>();
builder.Services.AddSingleton<IScreenshotRenderer, PdfRenderer>();
builder.Services.AddSingleton<IScreenshotRenderer, WebpRenderer>();
builder.Services.AddSingleton<ScreenshotRendererRegistry>();

builder.Services.AddSingleton<IViewportStrategy, DesktopViewportStrategy>();
builder.Services.AddSingleton<IViewportStrategy, MobileViewportStrategy>();
builder.Services.AddSingleton<IViewportStrategy, TabletViewportStrategy>();
builder.Services.AddSingleton<ViewportStrategyRegistry>();

builder.Services.AddSingleton<PuppeteerHelper>();

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<S3Settings>>().Value;

    var credentials = new BasicAWSCredentials(settings.AccessKey, settings.SecretKey);
    var config = new AmazonS3Config
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(settings.Region)
    };

    return new AmazonS3Client(credentials, config);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseMetricServer();
app.UseHttpMetrics();

app.UseAuthorization();

app.MapControllers();

app.Run();