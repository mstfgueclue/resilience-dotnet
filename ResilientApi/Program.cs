using Microsoft.EntityFrameworkCore;
using ResilientApi.Data;
using ResilientApi.Data.Repositories;

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
builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("CarsDb"));

var app = builder.Build();

// Access repositories to ensure seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    // Access the DatabaseSeeder and trigger the seeding
    var seederService = services.GetRequiredService<DatabaseSeeder>();
    seederService.Seed();
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
