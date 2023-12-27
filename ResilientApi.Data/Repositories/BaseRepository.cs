using Microsoft.Extensions.Logging;

namespace ResilientApi.Data.Repositories;

public abstract class BaseRepository(DataContext dbContext, ILogger logger)
{
    private readonly DataContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<bool> SaveChangesAsync()
    {
        _logger.LogInformation($"Saving the changes in the context.");

        // Return success if one or more rows were changed
        return await _dbContext.SaveChangesAsync() > 0;
    }
}