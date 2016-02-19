using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Security_ASPNetCore1_Angular2;

namespace Security_ASPNetCore1_JwtBearer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure authentication - who are you?
            services.AddAuthentication();

            // Configure authorization - what can you do?
            services.AddAuthorization(SecurityHelper.Authorization);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseCookieAuthentication(SecurityHelper.Authentication());

            app.UseMvc();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Path not found: " + context.Request.Path);
            });

            // todo
            //app.UseClaimsTransformation(user =>
            //{
            //    if (user.Identity.IsAuthenticated)
            //    {
            //        var claim = new Claim(ClaimTypes.Role, "admin");
            //        user.Identities.First().AddClaims(new[] { claim });
            //    }
            //    return Task.FromResult(user);
            //});
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
