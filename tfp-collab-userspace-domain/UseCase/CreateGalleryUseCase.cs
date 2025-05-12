using Microsoft.Extensions.Logging;
using tfp_collab_userspace_domain.Service;
using tfp_collab_userspace_interfaces.dto;
using tfp_collab_userspace_interfaces.Request;
using tfp_collab_userspace_interfaces.Response;

namespace tfp_collab_userspace_domain.UseCase;

public class CreateGalleryUseCase
    : ICreateGalleryUseCase
{
    private readonly IDatabaseService _databaseService;
    private readonly ILogger<CreateGalleryUseCase> _logger;

    public CreateGalleryUseCase(IDatabaseService databaseService, ILogger<CreateGalleryUseCase> logger)
    {
        _databaseService = databaseService;
        _logger = logger;
    }

    public async Task<CreateGalleryResponse> Handle(CreateGalleryRequest request)
    {
        try
        {
            GalleryDto newGallery = new ()
            {
                Name = request.Name, 
                OwnerId = request.OwnerId
            };
            await _databaseService.CreateGalleryAsync(newGallery);
            return new CreateGalleryResponse(newGallery.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create gallery");
            return new CreateGalleryResponse($"Error creating gallery: {ex.Message}");
        }
    }
}
