using tfp_collab_userspace_domain.Request;

namespace tfp_collab_userspace_domain.InputPorts;

public interface ICreateGalleryInputPort
{
    Task Handle(CreateGalleryRequest request, CancellationToken cancellationToken);
}