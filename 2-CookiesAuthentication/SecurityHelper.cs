using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http.Authentication;

namespace Security_ASPNetCore1_Angular2
{
    public class SecurityHelper
    {
        public const string CookieAuthenticationSchema = "CookieAuthenticationSchema";
        public const string AuthenticatedUserPolicy = "AuthenticatedUserPolicy";

        public static CookieAuthenticationOptions Authentication()
        {
            return new CookieAuthenticationOptions
            {
                AuthenticationScheme = CookieAuthenticationSchema,
                AutomaticChallenge = true
            };
        }

        public static void Authorization(AuthorizationOptions options)
        {
            options.AddPolicy(AuthenticatedUserPolicy, policy =>
            {
                policy.AuthenticationSchemes.Add(CookieAuthenticationSchema);
                policy.RequireAuthenticatedUser();
            });
        }

        public static async Task SignIn(AuthenticationManager authentication, string name)
        {
            // Create claims
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, name),
            }, CookieAuthenticationSchema);

            var user = new ClaimsPrincipal(identity);
            await authentication.SignInAsync(CookieAuthenticationSchema, user);
        }

        public static async Task SignOut(AuthenticationManager authentication)
        {
            await authentication.SignOutAsync(CookieAuthenticationSchema);
        }
    }
}
