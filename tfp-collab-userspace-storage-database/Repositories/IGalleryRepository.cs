using FluentResults;
using tfp_collab_userspace_storage_database.Model;

namespace tfp_collab_userspace_storage_database.Repositories;

public interface IGalleryRepository
    : IDisposable
{
    Task<Result<Gallery>> Get(Guid id);
    Task<Result<IEnumerable<Gallery>>> Get();
    Task<Result> CreateAsync(Gallery image);
    Task<Result> DeleteAsync(Guid id);
}