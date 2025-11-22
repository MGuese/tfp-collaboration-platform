namespace tfp_collab_userspace_storage_database.Model;

public class Gallery
{
    public Guid Id { get; set; }
    public string Name { get; init; } =  null!;
    public Guid OwnerId { get; init; }
    public DateTime AddedOn { get; set; }

}