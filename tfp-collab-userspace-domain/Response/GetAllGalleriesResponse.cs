using tfp_collab_userspace_domain.dto;

namespace tfp_collab_userspace_domain.Response;

public class GetAllGalleriesResponse
{
    public IEnumerable<GalleryDto> Galleries { get; }
    public bool IsSuccessful { get; }
    public string ErrorMessage { get; }

    public GetAllGalleriesResponse(IEnumerable<GalleryDto> galleries)
    {
        Galleries = galleries;
        IsSuccessful = true;
        ErrorMessage = null;
    }

    public GetAllGalleriesResponse(string errorMessage)
    {
        Galleries = null;
        IsSuccessful = false;
        ErrorMessage = errorMessage;
    }
}