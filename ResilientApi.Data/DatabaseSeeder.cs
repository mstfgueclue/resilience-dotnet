using ResilientApi.Data.Repositories;

namespace ResilientApi.Data;

public class DatabaseSeeder(ICarRepository carRepository, IOwnerRepository ownerRepository)
{
    public void Seed()
    {
        carRepository.GetAllCarsAsync().Wait();
        ownerRepository.GetAllOwnersAsync().Wait();
    }
}