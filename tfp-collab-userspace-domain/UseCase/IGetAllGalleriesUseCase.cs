using tfp_collab_userspace_interfaces.Request;
using tfp_collab_userspace_interfaces.Response;

namespace tfp_collab_userspace_domain.UseCase;

public interface IGetAllGalleriesUseCase
{
    Task<GetAllGalleriesResponse> Handle(GetAllGalleriesRequest request);
}