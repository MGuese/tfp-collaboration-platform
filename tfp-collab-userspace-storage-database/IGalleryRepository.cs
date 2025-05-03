using FluentResults;
using tfp_collab_userspace_storage_database.Model;

namespace tfp_collab_userspace_storage_database;

public interface IGalleryRepository
    : IDisposable
{
    Task<Result<Gallery>> GetByIdAsync(Guid id);
    Task<Result> CreateAsync(Gallery image);
    Task<Result> DeleteAsync(Guid id);
}