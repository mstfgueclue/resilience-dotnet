using Microsoft.EntityFrameworkCore;
using ResilientApi.Data;
using ResilientApi.Data.Repositories;
using ResilientApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<DatabaseSeeder>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("CarsDb"));

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    // Access the DatabaseSeeder and trigger the seeding
    var seederService = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seederService.Seed();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public abstract partial class Program;