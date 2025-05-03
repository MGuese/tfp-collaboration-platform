using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using tfp_collab_userspace_domain.InputPorts;
using tfp_collab_userspace_domain.Request;
using tfp_collab_userspace_domain.UseCase;

[ApiController]
[Route("api/[controller]")]
public class GalleryController : ControllerBase
{
    private readonly ICreateGalleryInputPort _createGalleryUseCase;
    private readonly CreateGalleryPresenter _presenter; // Inject the Presenter

    public GalleryController(ICreateGalleryInputPort createGalleryUseCase, CreateGalleryPresenter presenter)
    {
        _createGalleryUseCase = createGalleryUseCase;
        _presenter = presenter;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateGallery([FromBody] CreateGalleryRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // The Presenter is already injected and will receive the output
        await _createGalleryUseCase.Handle(request, cancellationToken);

        return _presenter.ViewModel;
    }
}