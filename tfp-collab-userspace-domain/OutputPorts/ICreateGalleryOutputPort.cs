using Microsoft.AspNetCore.Mvc;
using tfp_collab_userspace_domain.Response;

namespace tfp_collab_userspace_domain.OutputPorts;

public interface ICreateGalleryOutputPort
{
    IActionResult ViewModel { get; }
    Task Handle(CreateGalleryResponse response);
}