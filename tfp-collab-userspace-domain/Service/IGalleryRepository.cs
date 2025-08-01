using FluentResults;
using tfp_collab_userspace_domain.DomainObjects;

namespace tfp_collab_userspace_domain.Service;

public interface IGalleryRepository
{
    Task<Result<GalleryDo>> GetAsync(OwnerId ownerId, GalleryId id);
    Task<Result<IEnumerable<GalleryDo>>> GetAsync(OwnerId ownerId);
    Task<Result<GalleryDo>> CreateAsync(GalleryDo image);
    Task<Result> DeleteAsync(GalleryId id);
}