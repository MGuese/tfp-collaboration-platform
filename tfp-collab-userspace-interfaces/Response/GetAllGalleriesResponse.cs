using tfp_collab_userspace_interfaces.dto;

namespace tfp_collab_userspace_interfaces.Response;

public class GetAllGalleriesResponse
{
    public IEnumerable<GalleryDto> Galleries { get; }
    public bool IsSuccessful { get; }
    public string ErrorMessage { get; }

    public GetAllGalleriesResponse(IEnumerable<GalleryDto> galleries)
    {
        Galleries = galleries;
        IsSuccessful = true;
        ErrorMessage = string.Empty;
    }

    public GetAllGalleriesResponse(string errorMessage)
    {
        Galleries = Array.Empty<GalleryDto>();
        IsSuccessful = false;
        ErrorMessage = errorMessage;
    }
}