namespace Synith.Core.Interfaces;
public interface IEntityService<TRetrieveDto>
    where TRetrieveDto : class
{
    Task DeactivateAsync(int id);
    Task<IEnumerable<TRetrieveDto>> RetrieveAllAsync();
    Task<TRetrieveDto> RetrieveByIdAsync(int id);
}
