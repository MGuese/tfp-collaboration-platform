using FluentResults;
using tfpcollab.userspace.domain.DomainObjects;

namespace tfpcollab.userspace.domain.Service;

public interface IGalleryRepository
{
    Task<Result<GalleryDo>> GetAsync(OwnerId ownerId, GalleryId id);
    Task<Result<IEnumerable<GalleryDo>>> GetAsync(OwnerId ownerId);
    Task<Result<GalleryDo>> CreateAsync(GalleryDo image);
    Task<Result> DeleteAsync(GalleryId id);
}