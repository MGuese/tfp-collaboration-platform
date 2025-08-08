/*
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace tfp_collab_userspace_api.Authorization
{
    public class DevelopmentAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public DevelopmentAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Context.Request.Headers.ContainsKey("X-Development-Auth"))
            {
                // Wenn ein spezieller Header vorhanden ist, authentifiziere als Dummy-Benutzer
                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, "f9f0a1b2-c3d4-e5f6-7890-1234567890ab"), // Die Dummy-OwnerId
                    new Claim(ClaimTypes.Name, "devuser@example.com")
                    // Fügen Sie hier alle Claims hinzu, die Ihr CurrentUserContext oder Authorize-Policys erwarten
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            // Wenn kein Dev-Header vorhanden, scheitere die Authentifizierung
            return Task.FromResult(AuthenticateResult.Fail("No X-Development-Auth header present."));
        }
    }
}
*/