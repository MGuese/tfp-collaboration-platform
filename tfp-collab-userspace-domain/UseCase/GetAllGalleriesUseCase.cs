using Microsoft.Extensions.Logging;
using tfp_collab_userspace_domain.Service;
using tfp_collab_userspace_interfaces.Request;
using tfp_collab_userspace_interfaces.Response;

namespace tfp_collab_userspace_domain.UseCase;

public class GetAllGalleriesUseCase
    : IGetAllGalleriesUseCase
{
    private readonly IDatabaseService _databaseService;
    private readonly ILogger<GetAllGalleriesUseCase> _logger;

    public GetAllGalleriesUseCase(IDatabaseService databaseService, ILogger<GetAllGalleriesUseCase> logger)
    {
        _databaseService = databaseService;
        _logger = logger;
    }

    public async Task<GetAllGalleriesResponse> Handle(GetAllGalleriesRequest request)
    {
        try
        {
            var galleries = await _databaseService.GetAsync(request.OwnerId);
            return new GetAllGalleriesResponse(galleries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get galleries");
            return new GetAllGalleriesResponse($"Error retrieving galleries: {ex.Message}");
        }
    }
}