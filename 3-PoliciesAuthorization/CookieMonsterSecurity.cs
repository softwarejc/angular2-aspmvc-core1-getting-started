using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;

namespace Security_ASPNetCore1_Angular2
{
    public static class CookieMonsterSecurity
    {
        private const string CookieMonsterAuthenticationSchema = "CookieMonsterAuthenticationSchema";

        public const string OnlyGoodMonstersPolicy = "OnlyGoodMonstersPolicy";
        public const string MonsterTypeClaim = "MonsterTypeClaim";

        public static class MonsterTypes
        {
            public const string Good = "Good";
            public const string Bad = "Bad";
        }

        public static void Authorization(AuthorizationOptions options)
        {
            options.AddPolicy(CookieMonsterSecurity.OnlyGoodMonstersPolicy, policy =>
            {
                policy.AuthenticationSchemes.Add(CookieMonsterAuthenticationSchema);

                // Custom requirement
                policy.AddRequirements(new IsGoodMonsterRequirement());
            });
        }

        public static CookieAuthenticationOptions AuthenticationOptions()
        {
            return new CookieAuthenticationOptions
            {
                AuthenticationScheme = CookieMonsterSecurity.CookieMonsterAuthenticationSchema,
                AutomaticChallenge = true
            };
        }

        public static async Task SignIn(AuthenticationManager authentication, string monsterName)
        {
            // Create claims
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, monsterName),
                new Claim(CookieMonsterSecurity.MonsterTypeClaim, MonsterTypes.Good),
            }, CookieMonsterSecurity.CookieMonsterAuthenticationSchema);

            var user = new ClaimsPrincipal(identity);
            await authentication.SignInAsync(CookieMonsterSecurity.CookieMonsterAuthenticationSchema, user);
        }

        public static async Task SignOut(AuthenticationManager authentication)
        {
            await authentication.SignOutAsync(CookieMonsterSecurity.CookieMonsterAuthenticationSchema);
        }
    }
}
