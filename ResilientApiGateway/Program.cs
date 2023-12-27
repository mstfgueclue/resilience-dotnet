using ResilientApiGateway.HttpHelper;
using ResilientApiGateway.Strategies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<ClientStrategies>();
builder.Services.AddScoped<IApiRequestService, ApiRequestService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
// set base address for resilient client to localhost:5000
builder.Services.AddHttpClient("ResilientClient", 
    client => client.BaseAddress = new Uri("https://localhost:7266/"));

var app = builder.Build();

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
