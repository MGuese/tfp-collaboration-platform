using Microsoft.Extensions.Logging;
using tfp_collab_userspace_domain.DomainObjects;
using tfp_collab_userspace_domain.Request;
using tfp_collab_userspace_domain.Response;
using tfp_collab_userspace_domain.Service;

namespace tfp_collab_userspace_domain.UseCase;

public class CreateGalleryUseCase
    : ICreateGalleryUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateGalleryUseCase> _logger;

    public CreateGalleryUseCase(IUnitOfWork unitOfWork, ILogger<CreateGalleryUseCase> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CreateGalleryResponse> Handle(CreateGalleryRequest request)
    {
        try
        {
            GalleryDo newGallery = new ()
            {
                Name = request.Name, 
                OwnerId = (OwnerId)request.OwnerId
            };
            var createResult = await _unitOfWork.GalleryRepository.CreateAsync(newGallery);
            await _unitOfWork.SaveAsync();
            if (!createResult.IsFailed) return new CreateGalleryResponse(createResult.Value.Id);
            var errorMessages = createResult.Errors.Select(e => e.Message);
            // Verknüpfe alle Nachrichten mit einem Zeilenumbruch
            var allErrorsAsString = string.Join(Environment.NewLine, errorMessages);
            return new CreateGalleryResponse(allErrorsAsString);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create gallery");
            return new CreateGalleryResponse($"Error creating gallery: {ex.Message}");
        }
    }
}