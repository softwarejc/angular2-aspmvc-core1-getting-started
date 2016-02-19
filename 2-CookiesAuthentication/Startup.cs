using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Security_ASPNetCore1_JwtBearer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                // Automatically provide identity
                AutomaticAuthenticate = true,

                // This redirect to login
                AutomaticChallenge = true,

                LoginPath = new PathString("/api/user/login"),
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
