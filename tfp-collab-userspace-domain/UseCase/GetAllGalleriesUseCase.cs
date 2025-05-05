using tfp_collab_userspace_domain.dto;
using tfp_collab_userspace_domain.OutputPorts;
using tfp_collab_userspace_domain.Request;
using tfp_collab_userspace_domain.Response;
using tfp_collab_userspace_domain.Service;

namespace tfp_collab_userspace_domain.UseCase;

public class GetAllGalleriesUseCase : IGetAllGalleriesUseCase
{
    private readonly IDatabaseService _databaseService;
    private readonly IGetAllGalleriesOutputPort _outputPort;

    public GetAllGalleriesUseCase(IDatabaseService databaseService, IGetAllGalleriesOutputPort outputPort)
    {
        _databaseService = databaseService;
        _outputPort = outputPort;
    }

    public async Task Handle(GetAllGalleriesRequest request)
    {
        try
        {
            var galleries = await _databaseService.Get();
            await _outputPort.Handle(new GetAllGalleriesResponse(galleries));
        }
        catch (Exception ex)
        {
            // Log the error
            await _outputPort.Handle(new GetAllGalleriesResponse($"Error retrieving galleries: {ex.Message}"));
        }
    }
}

public interface IGetAllGalleriesUseCase
{
    Task Handle(GetAllGalleriesRequest request);
}