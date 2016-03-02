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
using Microsoft.AspNet.Authentication.OpenIdConnect;
using IdentityModel.Client;

namespace _5_OpenIdConnect
{
    public static class Constants
    {
        public const string Authority = "https://localhost:44300/"; // Identity Server
        public const string UserInfoEndpoint = Authority + "connect/userinfo";
        public const string TokenEndpoint = Authority + "connect/token";
        public const string ClientId = "mvc";
        public const string ClientSecret = "secret";
    }

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
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies",
                AutomaticAuthenticate = true, // OnAuthorizationCodeReceived will do the authentication
                AutomaticChallenge = false,
            });

            OpenIdConnectEvents events = null;
            app.UseOpenIdConnectAuthentication(options =>
            {
                options.AuthenticationScheme = "Oidc";
                options.AutomaticAuthenticate = false;
                options.AutomaticChallenge = true;
                options.SignInScheme = "Cookies"; // Middleware to persist user in a cookie

                options.Authority = Constants.Authority; // Identity Server
                options.ClientId = Constants.ClientId; // This application must be registered in identity server with this id
                options.ResponseType = "code id_token";
                options.GetClaimsFromUserInfoEndpoint = true;

                options.Scope.Add("offline_access");
                options.Scope.Add("role");

                // Used to register later events
                events = options.Events as OpenIdConnectEvents;
            });

            events.OnAuthorizationCodeReceived = OnAuthorizationCodeReceived;
            events.OnAuthenticationValidated = OnAuthenticationValidated;

            app.UseDeveloperExceptionPage();
            app.UseMvc();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Path not found: " + context.Request.Path);
            });
        }

        private async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            // 1) Use the code to get the access and refresh token, 
            // As we are using the hybrid we will get code and access token but not refresh token,
            // using the code we can get one if the client application is a server side app (like this example)
            var tokenClient = new TokenClient(Constants.TokenEndpoint, Constants.ClientId, Constants.ClientSecret);
            var tokensResponse = tokenClient.RequestAuthorizationCodeAsync(context.Code, context.RedirectUri).Result;
            List<Claim> oauthClaims = new List<Claim>();
            oauthClaims.Add(new Claim("access_token", tokensResponse.AccessToken)); // JWT token, This will allow us to call the Resource (WebAPI)
            oauthClaims.Add(new Claim("refresh_token", tokensResponse.RefreshToken));
            oauthClaims.Add(new Claim("expires_at", DateTime.Now.AddSeconds(tokensResponse.ExpiresIn).ToLocalTime().ToString()));

            // 2) Use the access token to retrieve user info claims
            var userInfoClient = new UserInfoClient(new Uri(Constants.UserInfoEndpoint), tokensResponse.AccessToken);
            var userInfo = await userInfoClient.GetAsync();
            List<Claim> userClaims = userInfo.Claims.Select(ui => new Claim(ui.Item1, ui.Item2)).ToList();

            // 3) Add claims to authentication ticket
            ClaimsIdentity identity = context.AuthenticationTicket.Principal.Identity as ClaimsIdentity;
            identity.AddClaims(oauthClaims);
            identity.AddClaims(userClaims);
        }

        private Task OnAuthenticationValidated(AuthenticationValidatedContext context)
        {
           Console.WriteLine("OnAuthenticationValidated");

            return Task.FromResult(0);
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
