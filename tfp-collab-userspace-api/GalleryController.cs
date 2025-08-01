using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tfp_collab_userspace_api.Authorization;
using tfp_collab_userspace_domain.Request;
using tfp_collab_userspace_domain.UseCase;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class GalleryController (ILogger<GalleryController> logger, ICurrentUserContext currentUserContext)
    : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateGalleryRequest request, 
        [FromServices] ICreateGalleryUseCase createGalleryUseCase)
    {
        request.OwnerId = currentUserContext.GetCurrentOwnerId();
        logger.LogDebug($"Received request for OwnerId: {request.OwnerId}, creating Gallery {request.Name}");
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