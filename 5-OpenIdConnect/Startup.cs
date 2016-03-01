using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNet.Http;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNet.Authentication.OpenIdConnect;

namespace _5_OpenIdConnect
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseDeveloperExceptionPage();
            app.UseIISPlatformHandler();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies",
                AutomaticAuthenticate = true,
                CookieName = "MyApp"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                Authority = "https://localhost:44300/", // Identity Server
                ClientId = "mvc", // This application must be registered in identity server with this id

                SignInScheme = "Cookies", // Middleware to persist user in a cookie
                AuthenticationScheme = "OpenId",

                AutomaticAuthenticate = true,
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
