namespace tfp_collab_userspace_domain.Response;

public class CreateGalleryResponse
{
    public Guid Id { get; }
    public string ErrorMessage { get; }
    public bool IsSuccessul { get; }

    public CreateGalleryResponse(Guid id)
    {
        Id = id;
        IsSuccessul = true;
    }

    public CreateGalleryResponse(string failure)
    {
        ErrorMessage = failure;
        IsSuccessul = false;
    }
}