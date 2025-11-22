namespace tfpcollab.userspace.infrastructure.database.Model;

public class Gallery
{
    public Guid Id { get; set; }
    public string Name { get; init; } =  null!;
    public Guid OwnerId { get; init; }
    public DateTime AddedOn { get; set; }

}