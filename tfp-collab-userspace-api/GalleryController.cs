using Microsoft.AspNetCore.Mvc;
using tfp_collab_userspace_domain.Request;
using tfp_collab_userspace_domain.UseCase;

[ApiController]
[Route("api/[controller]")]
public class GalleryController : ControllerBase
{
    private readonly ICreateGalleryUseCase _createGalleryUseCase;
    private readonly CreateGalleryPresenter _presenter; // Inject the Presenter

    public GalleryController(ICreateGalleryUseCase createGalleryUseCase, CreateGalleryPresenter presenter)
    {
        _createGalleryUseCase = createGalleryUseCase;
        _presenter = presenter;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateGalleryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // The Presenter is already injected and will receive the output
        await _createGalleryUseCase.Handle(request);

        return _presenter.ViewModel;
    }
    
}