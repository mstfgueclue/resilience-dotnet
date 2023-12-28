using Microsoft.Extensions.Logging;
using Moq;
using ResilientApi.Data.Models;
using ResilientApi.Data.Repositories;
using Xunit.Abstractions;

namespace ResilientApi.Test;

public class OwnerRepositoryTests(ITestOutputHelper output) : BaseTests(output)
{
    private static OwnerRepository CreateOwnerRepository()
    {
        var dbContext = GetInMemoryDbContext();
        var logger = new Mock<ILogger<OwnerRepository>>();
        return new OwnerRepository(dbContext, logger.Object);
    }
    
    [Fact]
    public async Task GetOwnerByIdAsync_NonExistingId_ShouldThrowException()
    {
        // Arrange
        var ownerRepository = CreateOwnerRepository();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => ownerRepository.GetOwnerByIdAsync(-1));
    }

    [Fact]
    public async Task GetOwnerWithCarsByIdAsync_ExistingId_ShouldReturnOwnerWithCars()
    {
        // Arrange
        var ownerRepository = CreateOwnerRepository();

        // Act
        var existingOwner = (await ownerRepository.GetAllOwnersAsync()).First();
        var result = await ownerRepository.GetOwnerWithCarsByIdAsync(existingOwner.Id);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Owner>(result);
        Assert.Equal(existingOwner.Id, result.Id);
        Assert.NotNull(result.Cars);
    }

    [Fact]
    public async Task CreateOwnerAsync_ShouldCreateNewOwner()
    {
        // Arrange
        var ownerRepository = CreateOwnerRepository();
        var newOwner = new Owner { Name = "New Owner" };

        // Act
        var result = await ownerRepository.CreateOwnerAsync(newOwner);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Owner>(result);
        Assert.Equal(newOwner.Name, result.Name);
    }

    [Fact]
    public async Task UpdateOwnerAsync_ExistingOwner_ShouldUpdateOwner()
    {
        // Arrange
        var ownerRepository = CreateOwnerRepository();
        var existingOwner = (await ownerRepository.GetAllOwnersAsync()).First();
        var updatedOwner = new Owner { Id = existingOwner.Id, Name = "Updated Owner" };

        // Act
        var result = await ownerRepository.UpdateOwnerAsync(updatedOwner);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Owner>(result);
        Assert.Equal(updatedOwner.Name, result.Name);
    }

    [Fact]
    public async Task UpdateOwnerAsync_NonExistingOwner_ShouldThrowException()
    {
        // Arrange
        var ownerRepository = CreateOwnerRepository();
        var nonExistingOwner = new Owner { Id = -1, Name = "Non-Existing Owner" };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => ownerRepository.UpdateOwnerAsync(nonExistingOwner));
    }

    [Fact]
    public async Task DeleteOwnerAsync_ExistingOwner_ShouldDeleteOwner()
    {
        // Arrange
        var ownerRepository = CreateOwnerRepository();
        var existingOwner = (await ownerRepository.GetAllOwnersAsync()).First();

        // Act
        await ownerRepository.DeleteOwnerAsync(existingOwner);

        // Assert
        await Assert.ThrowsAsync<Exception>(() => ownerRepository.GetOwnerByIdAsync(existingOwner.Id));
    }

    [Fact]
    public async Task DeleteOwnerAsync_NonExistingOwner_ShouldThrowException()
    {
        // Arrange
        var ownerRepository = CreateOwnerRepository();
        var nonExistingOwner = new Owner { Id = -1, Name = "Non-Existing Owner" };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => ownerRepository.DeleteOwnerAsync(nonExistingOwner));
    }
}