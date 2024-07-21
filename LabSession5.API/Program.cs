using Asp.Versioning;
using LabSession5.Application.Mappings;
using LabSession5.Application.Queries;
using LabSession5.Persistence.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UniversityContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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

app.MapControllers();

app.Run();