namespace tfp_collab_userspace_api.Authorization;

public interface ICurrentUserContext
{
    Guid GetCurrentOwnerId();
}