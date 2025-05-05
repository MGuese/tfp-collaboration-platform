using Microsoft.AspNetCore.Mvc;
using tfp_collab_userspace_domain.dto;
using tfp_collab_userspace_domain.Response;

namespace tfp_collab_userspace_domain.OutputPorts;

public interface IGetAllGalleriesOutputPort
{
    ActionResult<IEnumerable<GalleryDto>> ViewModel { get; }
    Task Handle(GetAllGalleriesResponse response);
}