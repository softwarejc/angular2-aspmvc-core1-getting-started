using System.Net.Http.Formatting;
using System.Web.Http;
using IdentityResourceAPI;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace IdentityResourceAPI
{
    public class Startup
    {
        private const string IdentityServerUrl = "https://localhost:44300";

        public void Configuration(IAppBuilder app)
        {
            // # 1) Bearer token authentication
            // Nuget source code and documentation https://github.com/IdentityServer/IdentityServer3.AccessTokenValidation
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                // accept only tokens issued by IdentityServer
                Authority = IdentityServerUrl,
                // accept only tokens that are issued for our API
                RequiredScopes = new[] { "tickets_api" }, // There is an scope defined in Identity Server that some clients can use 

                ValidationMode = ValidationMode.Local // JWT Local validation
            });

            // # 2) Web api 
            var config = new HttpConfiguration();

            // routing
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{action}");

            // JSON formatter
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            app.UseWebApi(config);
        }
    }
}
