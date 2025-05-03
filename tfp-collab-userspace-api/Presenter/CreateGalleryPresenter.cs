using Microsoft.AspNetCore.Mvc;
using tfp_collab_userspace_domain.OutputPorts;
using tfp_collab_userspace_domain.Response;

public class CreateGalleryPresenter : ICreateGalleryOutputPort
{
    public IActionResult ViewModel { get; private set; }

    public Task Handle(CreateGalleryResponse response)
    {
        ViewModel = response.Success
            ? (IActionResult)new CreatedAtActionResult(
                "CreateGallery", 
                "GalleryController", 
                new { response.Id }, 
                response)
            : new BadRequestObjectResult(new { response.FailureNumber });
        return Task.CompletedTask;
    }

}