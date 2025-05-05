using Microsoft.AspNetCore.Mvc;
using tfp_collab_userspace_domain.dto;
using tfp_collab_userspace_domain.OutputPorts;
using tfp_collab_userspace_domain.Request;
using tfp_collab_userspace_domain.UseCase;

[ApiController]
[Route("api/[controller]")]
public class GalleryController : ControllerBase
{
    private readonly ICreateGalleryUseCase _createGalleryUseCase;
    private readonly ICreateGalleryOutputPort _createGalleryPresenter;
    private readonly IGetAllGalleriesUseCase _getAllGalleriesUseCase;
    private readonly IGetAllGalleriesOutputPort _getAllGalleriesPresenter;

    public GalleryController(
        ICreateGalleryUseCase createGalleryUseCase, 
        ICreateGalleryOutputPort createGalleryPresenter, 
        IGetAllGalleriesUseCase getAllGalleriesUseCase,
        IGetAllGalleriesOutputPort getAllGalleriesPresenter)
    {
        _createGalleryUseCase = createGalleryUseCase;
        _createGalleryPresenter = createGalleryPresenter;
        _getAllGalleriesUseCase = getAllGalleriesUseCase;
        _getAllGalleriesPresenter = getAllGalleriesPresenter;
    }

    [HttpPost()]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateGalleryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _createGalleryUseCase.Handle(request);

        if (_createGalleryPresenter.ViewModel is CreatedAtActionResult createdResult)
        {
            // Hier könntest du ggf. die Id aus dem Response extrahieren, falls nötig
            return CreatedAtAction(createdResult.ActionName, createdResult.ControllerName, createdResult.RouteValues, createdResult.Value);
        }
        else if (_createGalleryPresenter.ViewModel is BadRequestObjectResult badRequestResult)
        {
            return BadRequest(badRequestResult.Value);
        }

        // Fallback, sollte nicht erreicht werden, wenn der Presenter korrekt arbeitet
        return StatusCode(500);
    }
    
    [HttpGet()]
    public async Task<ActionResult<IEnumerable<GalleryDto>>> Get([FromBody] GetAllGalleriesRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        // The Presenter is already injected and will receive the output
        await _getAllGalleriesUseCase.Handle(request);

        return _getAllGalleriesPresenter.ViewModel;
    }

}