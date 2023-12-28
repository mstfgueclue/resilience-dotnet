using Microsoft.Extensions.Logging;
using Moq;
using ResilientApi.Data;
using ResilientApi.Data.Models;
using ResilientApi.Data.Repositories;
using Xunit.Abstractions;

namespace ResilientApi.Test;

public class CarRepositoryTests(ITestOutputHelper output) : BaseTests(output)
{
    private static readonly DataContext DbContext = GetInMemoryDbContext();
    private static CarRepository CreateCarRepository()
    {
        var logger = new Mock<ILogger<CarRepository>>();
        return new CarRepository(DbContext, logger.Object);
    }
    
    [Fact]
    public async Task GetAllCarsAsync_ShouldReturnListOfCars()
    {
        // Arrange
        var carRepository = CreateCarRepository();

        // Act
        var result = await carRepository.GetAllCarsAsync();
        PrintJson(result);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<Car>>(result);
        Assert.True(result.Count > 0);
    }
    
    [Fact]
    public async Task GetCarByIdAsync_ExistingId_ShouldReturnCar()
    {
        // Arrange
        var carRepository = CreateCarRepository();

        // Act
        var existingCar = DbContext.Cars.First();
        PrintJson(existingCar);
        var result = await carRepository.GetCarByIdAsync(existingCar.Id);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Car>(result);
        Assert.Equal(existingCar.Id, result.Id);
    }
    
    [Fact]
    public async Task GetCarByIdAsync_NonExistingId_ShouldThrowException()
    {
        // Arrange
        var carRepository = CreateCarRepository();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => carRepository.GetCarByIdAsync(-1));
    }
}