using ResilientApi.Data.Models;

namespace ResilientApi.Data.Repositories;

public interface ICarRepository
{
    Task<List<Car?>> GetAllCarsAsync();
    Task<Car> GetCarByIdAsync(int id);
    Task<Car> CreateCarAsync(Car item);
    Task DeleteCarAsync(int id);
    Task<bool> UpdateCarAsync(Car car);
    
    Task<bool> AssignCarToOwnerAsync(int carId, int ownerId);
}