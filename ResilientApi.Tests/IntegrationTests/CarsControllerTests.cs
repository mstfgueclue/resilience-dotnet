using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ResilientApi.Data.Models;
using Xunit.Abstractions;

namespace ResilientApi.Tests.IntegrationTests;

public class CarsControllerTests(ITestOutputHelper output, WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Get_ReturnsOkResult()
    {
        // Act
        var response = await _client.GetAsync("/api/cars");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task Get_WithValidId_ReturnsOkResult()
    {
        // Act
        var response = await _client.GetAsync("/api/cars/1");
        var jsonResponse = await response.Content.ReadAsStringAsync();
        output.WriteLine("result" + jsonResponse);
        
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task Get_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/cars/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_CreatesNewCar_ReturnsCreatedAtRoute()
    {
        // Arrange
        var newCar = new Car
        {
            Make = "BMW",
            Model = "M5",
            Color = "Black",
            Year = 2023,
            Mileage = 1500,
            LicensePlate = "BMW001",
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now
        };
        var jsonContent = new StringContent(JsonConvert.SerializeObject(newCar), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/cars", jsonContent);
        // output response as json
        var responseContent = await response.Content.ReadAsStringAsync();
        output.WriteLine("result" + responseContent);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Put_WithValidId_UpdatesCar_ReturnsNoContent()
    {
        // Arrange
        var updatedCar = new Car
        {
            Make = "BMW",
            Model = "M6",
            Color = "Black",
            Year = 2023,
            Mileage = 200,
            LicensePlate = "BMW002",
        };
        var jsonContent = new StringContent(JsonConvert.SerializeObject(updatedCar), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/cars/1", jsonContent);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WithValidId_DeletesCar_ReturnsNoContent()
    {
        // Act
        var response = await _client.DeleteAsync("/api/cars/4");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task AssignCarToOwnerAsync_AssignsCarToOwner_ReturnsOkResult()
    {
        // Arrange
        const int carId = 1;
        const int ownerId = 1;

        // Act
        var response = await _client.PostAsync($"/api/cars/{carId}/owners/{ownerId}", null);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}