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

            if (user == null || !user.Identity.IsAuthenticated)
            {
                // Oder eine spezifischere Exception werfen, wenn der Benutzer nicht authentifiziert ist
                throw new InvalidOperationException("User is not authenticated. Cannot get OwnerId.");
            }

            // Annahme: Die OwnerId ist als ClaimTypes.NameIdentifier (Sub-Claim in JWT) gespeichert
            var ownerIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (ownerIdClaim == null || !Guid.TryParse(ownerIdClaim.Value, out Guid ownerId))
            {
                throw new InvalidOperationException("OwnerId claim not found or invalid format.");
            }

            return ownerId;
        }
    }
}