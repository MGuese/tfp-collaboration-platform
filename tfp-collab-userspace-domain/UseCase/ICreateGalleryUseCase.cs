using tfp_collab_userspace_domain.Request;
using tfp_collab_userspace_domain.Response;

namespace tfp_collab_userspace_domain.UseCase;

public interface ICreateGalleryUseCase
{
    Task<CreateGalleryResponse> Handle(CreateGalleryRequest request);
}