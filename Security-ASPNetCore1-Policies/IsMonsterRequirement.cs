using System;
using System.Diagnostics;
using Microsoft.AspNet.Authorization;

namespace Security_ASPNetCore1_Angular2
{
    public class IsGoodMonsterRequirement : AuthorizationHandler<IsGoodMonsterRequirement>, IAuthorizationRequirement
    {
        protected override void Handle(AuthorizationContext context, IsGoodMonsterRequirement requirement)
        {
            Console.WriteLine("Is a good monster?");

            if (!context.User.Identity.IsAuthenticated)
            {
                Console.WriteLine("... is authenticated...");
            }

            if (context.User.HasClaim(CookieMonsterSecurity.MonsterTypeClaim, CookieMonsterSecurity.MonsterTypes.Good))
            {
                Console.WriteLine("... and has the MonsterTypeClaim = MonsterTypes.Good!");
                context.Succeed(requirement);
            }
        }
    }
}