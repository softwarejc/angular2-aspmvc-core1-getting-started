using Microsoft.Owin;
using Owin;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services.InMemory;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer3.Core.Models;
using Serilog;

[assembly: OwinStartup(typeof(_4_IdentityServer.Startup))]

namespace _4_IdentityServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            // In Memory users
            var users = new List<InMemoryUser> {
                // Customer
                new InMemoryUser {
                    Username = "DemoCustomer",
                    Password = "customer123",
                    Subject = "1", // A special claim that is unique to that customer, it should never change
                    Claims = new List<Claim>
                    {
                        new Claim("name", "A customer"),
                        new Claim("email", "customer@email.com"),
                        new Claim("role", "customer")
                    },
                },

                // Administrator
                new InMemoryUser {
                    Username ="Administrator",
                    Password ="admin123",
                    Subject = "0", // A special claim that is unique to that customer, it should never change
                    Claims = new List<Claim>
                    {
                        new Claim("name", "Juan Carlos"),
                        new Claim("email", "juancarlos@email.com"),
                        new Claim("role", "admin")
                    },
                }
            };

            // Scopes (Claims): There are two types: Identity Scopes and Resources Scopes
            var scopes = new List<Scope>
            {
                // Standard Claims
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.Email,

                // Custom Claims
                new Scope
                {
                    Name = "roles",
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                }
            };

            // Clients = registered applications
            var clients = new List<Client>
            {
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Demo",
                    Flow = Flows.Hybrid, // OAuth2 protocol to be used
                    RedirectUris = new List<string>
                    {
                        // Where to redirect the call, the MVC address
                        "https://localhost:44333" + "/signin-oidc" // default Microsoft openId middleware callback path
                    },
                    AllowedScopes = new List<string>
                    {
                        // Information that the application can access,
                         StandardScopes.OpenId.Name,
                         StandardScopes.Email.Name,
                         StandardScopes.Profile.Name,
                         "roles"
                    }
                }
            };

            var factory = new IdentityServerServiceFactory();
            factory.UseInMemoryClients(clients);
            factory.UseInMemoryScopes(scopes);
            factory.UseInMemoryUsers(users);

            app.UseIdentityServer(new IdentityServerOptions {
                SiteName = "Identity Server Demo",
                SigningCertificate = SigningCertificate.Load(), // cetificate used to signin issued tokens
                Factory = factory // factory to find all users and resources
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Path not found: " + context.Request.Path);
            });
        }
    }
}
