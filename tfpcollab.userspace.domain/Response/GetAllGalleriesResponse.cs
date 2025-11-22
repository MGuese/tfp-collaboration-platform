using tfp_collab_userspace_domain.DomainObjects;

namespace tfp_collab_userspace_domain.Response;

public class GetAllGalleriesResponse
{
    public IEnumerable<GalleryDo> Galleries { get; }
    public bool IsSuccessful { get; }
    public string ErrorMessage { get; }

    public GetAllGalleriesResponse(IEnumerable<GalleryDo> galleries)
    {
        Galleries = galleries;
        IsSuccessful = true;
        ErrorMessage = string.Empty;
    }

    public GetAllGalleriesResponse(string errorMessage)
    {
        Galleries = [];
        IsSuccessful = false;
        ErrorMessage = errorMessage;
    }
}