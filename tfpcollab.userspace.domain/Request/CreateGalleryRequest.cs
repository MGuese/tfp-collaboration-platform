using System.ComponentModel.DataAnnotations;

namespace tfp_collab_userspace_domain.Request;

public class CreateGalleryRequest : ICreateGalleryRequest
{
    [Required]
    public Guid OwnerId { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than {1} characters.")]
    public string Name { get; set; } = null!;
}