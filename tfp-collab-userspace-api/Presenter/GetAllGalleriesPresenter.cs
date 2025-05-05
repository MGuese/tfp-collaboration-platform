using Microsoft.AspNetCore.Mvc;
using tfp_collab_userspace_domain.dto;
using tfp_collab_userspace_domain.OutputPorts;
using tfp_collab_userspace_domain.Response;

namespace tfp_collab_userspace_api.Presenter;

public class GetAllGalleriesPresenter 
    : IGetAllGalleriesOutputPort
{
    public ActionResult<IEnumerable<GalleryDto>> ViewModel { get; private set; }

    public Task Handle(GetAllGalleriesResponse response)
    {
        ViewModel = response.IsSuccessful
            ? (ActionResult<IEnumerable<GalleryDto>>)new CreatedAtActionResult(
                "Get", // Verwende den tatsächlichen Namen der Controller-Action
                "GalleryController",
                new { response.Galleries },
                response)
            : new BadRequestObjectResult(new { response.ErrorMessage });
        return Task.CompletedTask;
    }
}