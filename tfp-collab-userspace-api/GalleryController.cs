using Microsoft.AspNetCore.Mvc;
using tfp_collab_userspace_domain.dto;
using tfp_collab_userspace_domain.Request;
using tfp_collab_userspace_domain.UseCase;

[ApiController]
[Route("api/[controller]")]
public class GalleryController (ILogger<GalleryController> logger)
    : ControllerBase
{
    [HttpPost(Name = "Create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateGalleryRequest request, [FromServices] ICreateGalleryUseCase createGalleryUseCase)
    {
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
    
    [HttpGet()]
    public async Task<ActionResult<IEnumerable<GalleryDto>>> Get([FromBody] GetAllGalleriesRequest request, [FromServices] GetAllGalleriesUseCase getAllGalleriesUseCase)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        // The Presenter is already injected and will receive the output
        var getAllGalleriesResponse = await getAllGalleriesUseCase.Handle(request);

        if (getAllGalleriesResponse.IsSuccessful)
        {
            return Ok(getAllGalleriesResponse.Galleries);
        }
        return BadRequest(getAllGalleriesResponse.ErrorMessage);
    }
}