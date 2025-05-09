using tfp_collab_userspace_domain.dto;

namespace tfp_collab_userspace_domain.Service;

public interface IDatabaseService
{
    Task CreateGalleryAsync(GalleryDto gallery);
    Task<GalleryDto?> GetAsync(Guid id);
    Task<IEnumerable<GalleryDto>> GetAsync();
}