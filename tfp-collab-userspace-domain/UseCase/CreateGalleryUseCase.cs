using Microsoft.Extensions.Logging;
using tfp_collab_userspace_domain.dto;
using tfp_collab_userspace_domain.OutputPorts;
using tfp_collab_userspace_domain.Request;
using tfp_collab_userspace_domain.Response;
using tfp_collab_userspace_domain.Service;

namespace tfp_collab_userspace_domain.UseCase;

public interface ICreateGalleryUseCase
{
    Task Handle(CreateGalleryRequest request);
}

public class CreateGalleryUseCase
    : ICreateGalleryUseCase
{
    private readonly IDatabaseService _databaseService;
    private readonly ICreateGalleryOutputPort _outputPort;
    private readonly ILogger<CreateGalleryUseCase> _logger;

    public CreateGalleryUseCase(IDatabaseService databaseService, ICreateGalleryOutputPort outputPort, ILogger<CreateGalleryUseCase> logger)
    {
        _databaseService = databaseService;
        _outputPort = outputPort;
        _logger = logger;
    }

    public async Task Handle(CreateGalleryRequest request)
    {
        try
        {
            GalleryDto newGallery = new ()
            {
                Name = request.Name, 
                OwnerId = request.OwnerId
            };
            await _databaseService.CreateGalleryAsync(newGallery);
            await _outputPort.Handle(new CreateGalleryResponse(newGallery.Id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create gallery");
            await _outputPort.Handle(new CreateGalleryResponse(null, $"Error creating gallery: {ex.Message}"));
        }
    }
}
