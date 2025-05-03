namespace tfp_collab_userspace_domain.Response;

public class CreateGalleryResponse
{
    public Guid? Id { get; }
    public string FailureNumber { get; }
    public bool Success => Id is not null && Id.HasValue;

    public CreateGalleryResponse(Guid? id, string failureNumber = null)
    {
        Id = id;
        FailureNumber = failureNumber;
    }
}