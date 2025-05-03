namespace tfp_collab_userspace_storage_database.Model;

public class Gallery
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime AddedOn { get; set; }

}