using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Security_ASPNetCore1_JwtBearer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure athentication - who are you?
            services.AddAuthentication();

            // Configure authorization - what can you do?
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Automatic", policy =>
            //    {
            //        policy.AuthenticationSchemes.Add("Automatic");
            //        policy.RequireAuthenticatedUser();
            //    });
            //});

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                //AuthenticationScheme = "Automatic",
                AutomaticChallenge = true,
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
