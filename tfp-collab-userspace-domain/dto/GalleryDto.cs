namespace tfp_collab_userspace_domain.dto;

public class GalleryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime AddedOn { get; set; }
}