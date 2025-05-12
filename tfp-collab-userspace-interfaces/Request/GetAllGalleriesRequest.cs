using System.ComponentModel.DataAnnotations;

namespace tfp_collab_userspace_interfaces.Request;

public class GetAllGalleriesRequest : IGetAllGalleriesRequest
{
    [Required]
    public Guid OwnerId { get; set; }
}