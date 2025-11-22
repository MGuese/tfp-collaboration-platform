namespace tfp_collab_userspace_domain.DomainObjects;

public class GalleryDo
{
    public GalleryId Id { get; init; }
    public string Name { get; init; } = null!;
    public OwnerId OwnerId { get; init; }
    public DateTime AddedOn { get; init; }
}