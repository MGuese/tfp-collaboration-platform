namespace tfp_collab_userspace_interfaces.Request;

public interface ICreateGalleryRequest
{
    Guid OwnerId { get; set; }
    string Name { get; set; }
}