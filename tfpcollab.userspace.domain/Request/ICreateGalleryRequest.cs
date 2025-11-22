namespace tfpcollab.userspace.domain.Request;

public interface ICreateGalleryRequest
{
    Guid OwnerId { get; set; }
    string Name { get; set; }
}