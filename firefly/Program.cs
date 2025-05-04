using firefly.Data;
using firefly.Services;
using firefly.Services.Adapters;
using firefly.Services.Interfaces;
using firefly.Services.Storage;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.Local.json", optional: true);
builder.Services.AddControllers()
.AddJsonOptions(options =>
 {
     options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
     options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
 });

builder.Services.AddHttpClient<FireflyAdapter>().AddStandardResilienceHandler();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<FireflyStorageService>();
builder.Services.AddScoped<IImageService, FireflyAdapter>();
builder.Services.AddScoped<IImageGenerationJobRepository, ImageGenerationJobRepository>();


builder.Services.AddDbContext<LuminarDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<JobPollingService>();
builder.Services.AddScoped<IStorageService, AzureBlobStorageService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
