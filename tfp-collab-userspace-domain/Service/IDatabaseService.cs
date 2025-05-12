using tfp_collab_userspace_interfaces.dto;

namespace tfp_collab_userspace_domain.Service;

public interface IDatabaseService
{
    Task CreateGalleryAsync(GalleryDto gallery);
    Task<GalleryDto?> GetAsync(Guid ownerId, Guid galleryId);
    Task<IEnumerable<GalleryDto>> GetAsync(Guid ownerId);
}