using System.Globalization;
using System.Net;
using System.Net.Mail;
using Asp.Versioning;
using Hangfire;
using Hangfire.PostgreSql;
using HealthChecks.Network.Core;
using HealthChecks.UI.Client;
using LabSession5.Application.Mappings;
using LabSession5.Application.Queries;
using LabSession5.Application.Services;
using LabSession5.Infrastructure.Services;
using LabSession5.Persistence.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UniversityContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register Services
builder.Services.AddTransient<GradeService>();

// Configure localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Register API versioning
var apiVersioningSettings = builder.Configuration.GetSection("ApiVersioning");
var defaultApiVersion = new ApiVersion(apiVersioningSettings.GetValue<int>("MajorVersion"), apiVersioningSettings.GetValue<int>("MinorVersion"));
var assumeDefaultVersionWhenUnspecified = apiVersioningSettings.GetValue<bool>("AssumeDefaultVersionWhenUnspecified");

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = assumeDefaultVersionWhenUnspecified;
    options.DefaultApiVersion = defaultApiVersion;
});

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetCourseById).Assembly));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(UniversityProfile).Assembly);

// Register Caching
builder.Services.AddMemoryCache();

// Register Health Checks
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .AddSmtpHealthCheck(options =>
    {
        options.Host = builder.Configuration["Smtp:Host"];
        options.Port = int.Parse(builder.Configuration["Smtp:Port"]);
        options.ConnectionType = SmtpConnectionType.PLAIN; // No STARTTLS
        options.AllowInvalidRemoteCertificates = true;
        options.LoginWith(builder.Configuration["Smtp:User"], builder.Configuration["Smtp:Pass"]);
    }, name: "smtp")
    .AddUrlGroup(new Uri("https://google.com"), name: "google URL");

// Register HealthChecks UI
builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();

// Register Hangfire
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

builder.Services.AddSingleton<SmtpClient>(_ =>
{
    var client = new SmtpClient(builder.Configuration["Smtp:Host"], int.Parse(builder.Configuration["Smtp:Port"]));
    client.UseDefaultCredentials = false;
    client.Credentials = new NetworkCredential(builder.Configuration["Smtp:User"], builder.Configuration["Smtp:Pass"]);
    return client;
});
builder.Services.AddTransient<EmailNotificationService>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.UseDeveloperExceptionPage();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("fr"), new CultureInfo("de") },
    SupportedUICultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("fr"), new CultureInfo("de") }
});

app.MapControllers();

// Map Health Check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(setup =>
{
    setup.UIPath = "/health-ui";
    setup.ApiPath = "/health-ui-api";
});

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<EmailNotificationService>(service => 
        service.SendEmailAsync("serge@example.inmind.com", "Scheduled Email", "This is a scheduled email, every one minute you will receive it, good luck"), 
    "0 */5 * * * *"); // Every 5 minutes

app.Run();