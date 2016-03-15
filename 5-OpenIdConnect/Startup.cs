using System;
using System.Collections.Generic;
using System.Globalization;
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
                AuthenticationScheme = "Cookies", // We can have more than one authentication "system", we need a name to distinguish them

                AutomaticAuthenticate = true, // Way in - IF true, Convert cookie into identity object
                AutomaticChallenge = false, // Way out - IF true, Redirect to challenge URL
            });

            OpenIdConnectEvents events = null;
            app.UseOpenIdConnectAuthentication(options =>
            {
                options.AuthenticationScheme = "Oidc";
                options.AutomaticAuthenticate = false;
                options.AutomaticChallenge = true;
                options.SignInScheme = "Cookies"; // Middle-ware to persist user in a cookie

                options.Authority = Constants.Authority; // Identity Server
                options.ClientId = Constants.ClientId; // This application must be registered in identity server with this id
                options.ResponseType = "code id_token";
                options.GetClaimsFromUserInfoEndpoint = true;

                options.Scope.Add("offline_access"); // this scope is needed to get the refresh token
                options.Scope.Add("role");

                // Used to register events later
                events = options.Events as OpenIdConnectEvents;
            });

            if (events != null) events.OnAuthorizationCodeReceived = OnAuthorizationCodeReceived;

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
            // As we are using the hybrid flow, we will get a "code" and "access_token" but not "refresh_token".
            // Using the code we can get a "refresh_token" if the client application is a server side app (like this example)
            // If the application is a SPA or a native phone app, it is not secure to use the ClientSecret 
            var tokenClient = new TokenClient(Constants.TokenEndpoint, Constants.ClientId, Constants.ClientSecret);
            var tokensResponse = tokenClient.RequestAuthorizationCodeAsync(context.Code, context.RedirectUri).Result;

            var expiration = DateTime.Now.AddSeconds(tokensResponse.ExpiresIn)
               .ToLocalTime()
               .ToString(CultureInfo.InvariantCulture);

            List<Claim> oauthClaims = new List<Claim>
            {
                new Claim("access_token", tokensResponse.AccessToken),
                new Claim("refresh_token", tokensResponse.RefreshToken),
                new Claim("expires_at", expiration)
            };
            
            // 2) Use the access token to retrieve user info claims
            // The access token is a JWT token, it can be used to secure WebApi
            var userInfoClient = new UserInfoClient(new Uri(Constants.UserInfoEndpoint), tokensResponse.AccessToken);
            var userInfo = await userInfoClient.GetAsync();
            List<Claim> userClaims = userInfo.Claims.Select(ui => new Claim(ui.Item1, ui.Item2)).ToList();

            // 3) Add claims to authentication ticket
            ClaimsIdentity identity = context.AuthenticationTicket.Principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                // Remove all protocol related claims
                var claimsToRemove = identity.Claims.ToList();
                foreach (var claim in claimsToRemove)
                {
                    identity.RemoveClaim(claim);
                }

                // Add oauth and user claims
                identity.AddClaims(oauthClaims);
                identity.AddClaims(userClaims);
            }
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
