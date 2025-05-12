namespace tfp_collab_userspace_interfaces.Request;

public interface IGetAllGalleriesRequest
{
    Guid OwnerId { get; set; }
}