using tfp_collab_userspace_interfaces.dto;

namespace tfp_collab_userspace_domain.Service;

public interface IDatabaseService
{
    Task<GalleryDto?> CreateGalleryAsync(GalleryDto gallery);
}