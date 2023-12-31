using ResilientApi.Data.Repositories;

namespace ResilientApi.Data;

public class DatabaseSeeder(ICarRepository carRepository, IOwnerRepository ownerRepository)
{
    public async Task Seed()
    {
        await carRepository.GetAllCarsAsync();
        await ownerRepository.GetAllOwnersAsync();
    }
}