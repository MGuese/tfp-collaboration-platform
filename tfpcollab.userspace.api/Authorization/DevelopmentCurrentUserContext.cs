namespace tfpcollab.userspace.api.Authorization;

public class DevelopmentCurrentUserContext : ICurrentUserContext
{
    public Guid GetCurrentOwnerId()
    {
        return Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
    }
}