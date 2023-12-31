using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResilientApi.Data.Exceptions;
using ResilientApi.Data.Models;

namespace ResilientApi.Data.Repositories;

public class CarRepository : BaseRepository, ICarRepository
{
    private int _nextId;
    private readonly DataContext _dbContext;
    private readonly ILogger<CarRepository> _logger;

    public CarRepository(DataContext dbContext, ILogger<CarRepository> logger) : base(dbContext, logger)
    {
        _logger = logger;
        _dbContext = dbContext;
        
        SeedDatabaseAsync().Wait();
    }
    
    private async Task SeedDatabaseAsync()
    {
        if (_dbContext.Cars.Any()) return;

        _logger.LogInformation($"Seeding the database with sample car data");
        var cars = new List<Car>
        {
            new()
            {
                Make = "Ford",
                Model = "Model T",
                Color = "Black",
                Year = 1920,
                Mileage = 5000,
                LicensePlate = "OLD123",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
            },
            new()
            {
                Make = "Ford",
                Model = "Mustang",
                Color = "Red",
                Year = 2022,
                Mileage = 10000,
                LicensePlate = "FANCY001",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
            },
            new()
            {
                Make = "Ferrari",
                Model = "488 GTB",
                Color = "Yellow",
                Year = 2021,
                Mileage = 500,
                LicensePlate = "FERRARI001",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            },
            new()
            {
                Make = "Mercedes-Benz",
                Model = "S-Class",
                Color = "Silver",
                Year = 2023,
                Mileage = 1500,
                LicensePlate = "MBZLUX001",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            }
        };
        _nextId = cars.Count;
        _dbContext.Cars.AddRange(cars);
        await SaveChangesAsync();
    }

    public async Task<List<Car?>> GetAllCarsAsync()
    {
        _logger.LogInformation($"Retrieving all cars from the context.");

        return await _dbContext.Cars
            .Include(a => a.Owner)
            .DefaultIfEmpty()
            .ToListAsync();
    }

    public async Task<Car> GetCarByIdAsync(int id)
    {
        _logger.LogInformation($"Retrieving a car from the context.");

        var foundCar = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == id);
        if (foundCar == null)
        {
            throw new NotFoundException($"Car with id {id} not found");
        }

        return foundCar;
    }

    public async Task<Car> CreateCarAsync(Car car)
    {
        _logger.LogInformation($"Adding a new car to the context.");

        if (car == null)
        {
            throw new BadRequestException("car cannot be null");
        }

        car.Id = _nextId++;
        _dbContext.Cars.Add(car);
        await SaveChangesAsync();

        return car;
    }

    public async Task DeleteCarAsync(int id)
    {
        _logger.LogInformation($"Removing a car from the context.");

        var foundCar = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == id);
        if (foundCar == null)
        {
            throw new NotFoundException($"Car with id {id} not found");
        }

        _dbContext.Cars.Remove(foundCar);
        await SaveChangesAsync();
    }

    public async Task<bool> UpdateCarAsync(Car car)
    {
        _logger.LogInformation($"Updating a car in the context.");

        var foundCar = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == car.Id);
        if (foundCar == null)
        {
            throw new NotFoundException($"Car with id {car.Id} not found");
        }

        foundCar.Make = car.Make;
        foundCar.Model = car.Model;
        foundCar.Color = car.Color;
        foundCar.Year = car.Year;
        foundCar.Mileage = car.Mileage;
        foundCar.LicensePlate = car.LicensePlate;
        foundCar.DateUpdated = DateTime.Now;

        return await SaveChangesAsync();
    }

    public async Task<bool> AssignCarToOwnerAsync(int carId, int ownerId)
    {
        _logger.LogInformation($"Assigning a car to an owner in the context.");

        var foundCar = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == carId);
        if (foundCar == null)
        {
            throw new NotFoundException($"Car with id {carId} not found");
        }

        var foundOwner = await _dbContext.Owners.FirstOrDefaultAsync(o => o.Id == ownerId);
        if (foundOwner == null)
        {
            throw new NotFoundException($"Owner with id {ownerId} not found");
        }

        foundCar.OwnerId = foundOwner.Id;
        foundCar.Owner = foundOwner;
        foundCar.DateUpdated = DateTime.Now;

        return await SaveChangesAsync();
    }
}