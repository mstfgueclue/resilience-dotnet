using ResilientApi.Data.Models;

namespace ResilientApi.Data.Repositories;

public interface IOwnerRepository
{
    Task<IEnumerable<Owner>> GetAllOwnersAsync();
    
    Task<Owner> GetOwnerByIdAsync(int id);
    
    Task<Owner> GetOwnerWithCarsByIdAsync(int id);
    
    Task<Owner> CreateOwnerAsync(Owner owner);
    
    Task<Owner> UpdateOwnerAsync(Owner owner);
    
    Task DeleteOwnerAsync(Owner owner);
}