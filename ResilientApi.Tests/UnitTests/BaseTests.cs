using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ResilientApi.Data;
using Xunit.Abstractions;

namespace ResilientApi.Tests.UnitTests;

public class BaseTests(ITestOutputHelper output)
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

    protected static DataContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase("CarsDb")
            .Options;

        return new DataContext(options);
    }
    
    protected void PrintJson(object obj)
    {
        output.WriteLine(JsonSerializer.Serialize(obj, JsonSerializerOptions));
    }
}