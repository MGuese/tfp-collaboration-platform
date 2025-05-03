using tfp_collab_userspace_domain.Request;

namespace tfp_collab_userspace_domain.UseCase;

public interface ICreateGalleryUseCase
{
    Task Handle(CreateGalleryRequest request);
}