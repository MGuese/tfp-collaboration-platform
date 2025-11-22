namespace tfpcollab.userspace.api.Authorization;

public interface ICurrentUserContext
{
    Guid GetCurrentOwnerId();
}