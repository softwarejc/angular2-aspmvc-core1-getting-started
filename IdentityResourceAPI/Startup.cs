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
            GlobalConfiguration.Configure(ConfigureWebApi);

            // Configure Bearer token authentication
            // Nuget source code and documentation https://github.com/IdentityServer/IdentityServer3.AccessTokenValidation
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = IdentityServerUrl,
                RequiredScopes = new[] { "tickets_api" } // There is an scope defined in Identity Server that some clients can use 
            });
        }

        public static void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{action}");
        }
    }
}
