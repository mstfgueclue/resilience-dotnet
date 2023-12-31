using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResilientApi.Data.Exceptions;
using ResilientApi.Data.Models;

namespace ResilientApi.Data.Repositories;

public class OwnerRepository : BaseRepository, IOwnerRepository
{
    private int _nextId;
    private readonly DataContext _dbContext;
    private readonly ILogger<OwnerRepository> _logger;

    public OwnerRepository(DataContext dbContext, ILogger<OwnerRepository> logger) : base(dbContext, logger)
    {
        _logger = logger;
        _dbContext = dbContext;
        
        SeedDatabaseAsync().Wait();
    }
    
    private async Task SeedDatabaseAsync()
    {
        if (_dbContext.Owners.Any()) return;

        _logger.LogInformation($"Seeding the database with sample owner data");
        var owners = new List<Owner>
        {
            new()
            {
                Name = "John Doe",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
            },
            new()
            {
                Name = "Jane Doe",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            },
            new()
            {
                Name = "Bob Smith",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            },
            new()
            {
                Name = "Alice Smith",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            }
        };
        _nextId = owners.Count;
        _dbContext.Owners.AddRange(owners);
        await SaveChangesAsync();
    }

    public async Task<IEnumerable<Owner>> GetAllOwnersAsync()
    {
        _logger.LogInformation($"Retrieving all owners");
        
        return await _dbContext.Owners.ToListAsync();
    }

    public async Task<Owner> GetOwnerByIdAsync(int id)
    {
        _logger.LogInformation($"Retrieving owner by Id");
        
        var owner = await _dbContext.Owners.FindAsync(id);
        if (owner == null)
        {
            throw new NotFoundException($"Owner with id {id} not found");
        }
        
        return owner;
    }

    public async Task<Owner> GetOwnerWithCarsByIdAsync(int id)
    {
        _logger.LogInformation($"Retrieving owner by Id including cars");
        
        var owner = await _dbContext.Owners
            .Include(o => o.Cars)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (owner == null)
        {
            throw new NotFoundException($"Owner with id {id} not found");
        }
        
        return owner;
    }

    public async Task<Owner> CreateOwnerAsync(Owner owner)
    {
        _logger.LogInformation($"Adding a new owner to the context.");
        
        if (owner == null)
        {
            throw new BadRequestException("owner cannot be null");
        }

        owner.Id = _nextId++;
        _dbContext.Owners.Add(owner);
        await _dbContext.SaveChangesAsync();

        return owner;
    }

    public async Task<Owner> UpdateOwnerAsync(Owner owner)
    {
        _logger.LogInformation($"Updating an owner in the context.");
        
        if (owner == null)
        {
            throw new BadRequestException("owner cannot be null");
        }

        var foundOwner = await _dbContext.Owners.FindAsync(owner.Id);
        if (foundOwner == null)
        {
            throw new NotFoundException($"Owner with id {owner.Id} not found");
        }

        foundOwner.Name = owner.Name;
        foundOwner.DateUpdated = DateTime.Now;
        await _dbContext.SaveChangesAsync();

        return foundOwner;
    }

    public async Task DeleteOwnerAsync(Owner owner)
    {
        _logger.LogInformation($"Removing an owner from the context.");
        
        if (owner == null)
        {
            throw new ArgumentException("owner cannot be null");
        }

        var foundOwner = await _dbContext.Owners.FindAsync(owner.Id);
        if (foundOwner == null)
        {
            throw new NotFoundException($"Owner with id {owner.Id} not found");
        }

        _dbContext.Owners.Remove(foundOwner);
        await SaveChangesAsync();
    }
}