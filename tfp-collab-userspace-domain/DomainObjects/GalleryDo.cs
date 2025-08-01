namespace tfp_collab_userspace_domain.DomainObjects;

public class GalleryDo
{
    public GalleryId Id { get; set; }
    public string Name { get; set; }
    public OwnerId OwnerId { get; set; }
    public DateTime AddedOn { get; set; }
}