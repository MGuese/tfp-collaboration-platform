using Microsoft.AspNetCore.Mvc;
using tfpcollab.userspace.api.Authorization;
using tfpcollab.userspace.domain.Request;
using tfpcollab.userspace.domain.UseCase;

namespace tfpcollab.userspace.api;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class GalleryController(ILogger<GalleryController> logger)
    : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateGalleryRequest request, 
        [FromServices] ICreateGalleryUseCase createGalleryUseCase,
        [FromServices] ICurrentUserContext currentUserContext)
    {
        request.OwnerId = currentUserContext.GetCurrentOwnerId();
        logger.LogDebug("Received request for OwnerId: {RequestOwnerId}, creating Gallery {RequestName}", request.OwnerId, request.Name);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var createGalleryResponse = await createGalleryUseCase.Handle(request);

        if (createGalleryResponse.IsSuccessul)
        {
            return Ok(createGalleryResponse.Id);
        }
        return BadRequest(createGalleryResponse.ErrorMessage);
    }
}