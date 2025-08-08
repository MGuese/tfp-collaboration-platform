using System.Security.Claims;

namespace tfp_collab_userspace_api.Authorization
{
    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCurrentOwnerId()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user is null) throw new ArgumentNullException("user of HttpContext is null.");
                
            if(user.Identity is not null && !user.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated. Cannot get OwnerId.");
            }

            // Annahme: Die OwnerId ist als ClaimTypes.NameIdentifier (Sub-Claim in JWT) gespeichert
            var ownerIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (ownerIdClaim == null || !Guid.TryParse(ownerIdClaim.Value, out var ownerId))
            {
                throw new InvalidOperationException("OwnerId claim not found or invalid format.");
            }

            return ownerId;
        }
    }
}