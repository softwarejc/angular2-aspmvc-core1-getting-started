using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace Security_ASPNetCore1_Angular2
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure authentication - who are you?
            services.AddAuthentication();

            // Configure authorization - what can you do?
            services.AddAuthorization(CookieMonsterSecurity.Authorization);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseCookieAuthentication(CookieMonsterSecurity.AuthenticationOptions());

            app.UseClaimsTransformation(user =>
            {
                if (user.Identity.IsAuthenticated)
                {
                    var claim = new Claim("Dynamic Claim", "this claim was created using the middleware 'UseClaimsTransformation'");
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
}
