namespace tfpcollab.userspace.domain.Response;

public class CreateGalleryResponse
{
    public Guid Id { get; }
    public string ErrorMessage { get; }
    public bool IsSuccessul { get; }

    public CreateGalleryResponse(Guid id)
    {
        Id = id;
        IsSuccessul = true;
        ErrorMessage = string.Empty;
    }

    public CreateGalleryResponse(string failure)
    {
        ErrorMessage = failure;
        IsSuccessul = false;
    }
}