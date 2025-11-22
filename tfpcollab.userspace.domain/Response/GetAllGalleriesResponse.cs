using tfpcollab.userspace.domain.DomainObjects;

namespace tfpcollab.userspace.domain.Response;

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