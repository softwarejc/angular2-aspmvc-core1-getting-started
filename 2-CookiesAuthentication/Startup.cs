using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace Security_ASPNetCore1_JwtBearer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add required services for authentication - who are you?
            services.AddAuthentication();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            // Configure authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = SecurityConfig.Scheme,
                AutomaticChallenge = true,
            });

            app.UseClaimsTransformation(user =>
            {
                if (user.Identity.IsAuthenticated)
                {
                    var claim = new Claim(ClaimTypes.Role, "admin");
                    user.Identities.First().AddClaims(new[] { claim });
                }
                return Task.FromResult(user);
            });

            app.UseMvc();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Path not found: " + context.Request.Path);
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }

    public class SecurityConfig
    {
        public const string Scheme = "ABC";
    }

}
