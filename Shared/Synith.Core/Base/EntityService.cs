using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Synith.Core.Extensions;
using Synith.Core.Interfaces;
using Synith.Core.Messages;

namespace Synith.Core.Base;
public abstract class EntityService<TEntity, TRetrieveDto> : IEntityService<TRetrieveDto>
    where TEntity : Entity, new()
    where TRetrieveDto : class
{
    private readonly ILogger<EntityService<TEntity, TRetrieveDto>> _logger;
    private readonly DbContext _context;

    protected EntityService(ILogger<EntityService<TEntity, TRetrieveDto>> logger, DbContext context)
    {
        _logger = logger;
        _context = context;
    }

    protected abstract TRetrieveDto ToRetrieveDto(TEntity entity);

    /// <summary>
    /// Decrypt encrypted properties before returning the data.
    /// </summary>
    protected virtual TRetrieveDto Decrypt(TRetrieveDto entity) => entity;

    protected delegate IQueryable<TRetrieveDto> EntityQueryDelegate(IQueryable<TEntity> entities);
    /// <summary>
    /// Explicitly define the columns to be retrieved from the database.
    /// </summary>
    protected event EntityQueryDelegate? OnBeforeRetrieve;

    public async Task DeactivateAsync(int id)
    {
        _logger.LogInformation("Deactivating entity({id}).", id);

        TEntity? entity = await _context.Set<TEntity>()
            .Where(x => x.Id == id)
            .Select(x => new TEntity() { Id = x.Id }) // retrieve only the id to validate existence
            .SingleOrDefaultAsync();

        if (entity == null)
        {
            _logger.LogError("No entity({id}) was found.", id);
            throw new KeyNotFoundException(ErrorMessageProvider.NotFound($"{nameof(Entity)}.{nameof(Entity.Id)}", id.ToString()));
        }

        entity.Status = Enums.GenericStatus.Inactive;
        entity.InactiveDate = DateTime.UtcNow;

        _context.Entry(entity).Property(nameof(Entity.Status)).IsModified = true;
        _context.Entry(entity).Property(nameof(Entity.InactiveDate)).IsModified = true;
        
        await _context.SaveChangesAsync();
        _logger.LogDebug("Entity({id}) have been successfully deactivated.", id);
    }

    public virtual async Task<IEnumerable<TRetrieveDto>> RetrieveAllAsync()
    {
        _logger.LogDebug("Retrieving all entities.");

        IQueryable<TEntity> query = _context.Set<TEntity>().Where(x => x.Status != Enums.GenericStatus.Inactive);
        IEnumerable<TRetrieveDto> result;

        if (OnBeforeRetrieve == null)
        {
            IEnumerable<TEntity> entities = await query.ToListAsync();
            result = entities.Map(ToRetrieveDto);
        }
        else
        {
            _logger.LogDebug("Custom select query found.");
            result = await OnBeforeRetrieve.Invoke(query).ToListAsync();
        }

        result = result.Select(Decrypt);
        _logger.LogDebug("Entities have been successfully retrieved.");

        return result;
    }

    public virtual async Task<TRetrieveDto> RetrieveByIdAsync(int id)
    {
        _logger.LogDebug("Retrieving entity({id}).", id);

        TRetrieveDto? result = null;

        if (OnBeforeRetrieve == null)
        {
            TEntity? entity = await _context.Set<TEntity>().FindAsync(id);
            
            if (entity != null)
            {
                result = ToRetrieveDto(entity);
            }
        }
        else
        {
            _logger.LogDebug("Custom select query found.");
            result = await OnBeforeRetrieve.Invoke(_context.Set<TEntity>().Where(x => x.Id == id)).SingleOrDefaultAsync();
        }

        if (result == null)
        {
            _logger.LogError("No entity({id}) was found.", id);
            throw new KeyNotFoundException(ErrorMessageProvider.NotFound($"{nameof(Entity)}.{nameof(Entity.Id)}", id.ToString()));
        }

        result = Decrypt(result);
        _logger.LogDebug("Entity({id}) have been successfully retrieved.", id);

        return result;
    }
}
