﻿using Microsoft.Owin;
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

            // ## Users (In Memory)
            var users = new List<InMemoryUser> {
                // Customer - Bob
                new InMemoryUser {
                    Username = "DemoCustomer",
                    Password = "customer123",
                    Subject = "1", // A special claim that is unique to that customer, it should never change
                    Claims = new List<Claim>
                    {
                        new Claim("name", "Bob Smith"),
                        new Claim("email", "customer@email.com"),
                        new Claim("role", "customer")
                    },
                },

                // Administrator - Juan Carlos
                new InMemoryUser {
                    Username ="Administrator",
                    Password ="admin123",
                    Subject = "0", // A special claim that is unique to that customer, it should never change
                    Claims = new List<Claim>
                    {
                        new Claim("name", "Juan Carlos"),
                        new Claim("gender", "male"),
                        new Claim("nickname", "juasan00"),
                        new Claim("email", "juancarlos@email.com"),
                        new Claim("role", "admin")
                    },
                }
            };

            // ## Scopes (Claims): There are two types: Identity Scopes and Resources Scopes (WebApis)
            var scopes = new List<Scope>
            {
                // Standard Claims
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.Email,
                StandardScopes.OfflineAccess, // This allows refresh tokens: https://identityserver.github.io/Documentation/docsv2/advanced/refreshTokens.html

                // Custom Claims
                // Identity: Role 
                new Scope
                {
                    Name = "role",
                    DisplayName = "Role",
                    Description = "Your role",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                },

                 // Resource API
                new Scope
                {
                    Name = "tickets_api",
                    DisplayName = "Tickets api",
                    Description = "This is an API to get and create tickets",
                    Type = ScopeType.Resource,
                }
            };

            // ## Clients = registered applications
            var clients = new List<Client>
            {
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Demo",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    //RequireConsent = false, // consent makes sense only for 3rd party apps
                    Flow = Flows.Hybrid, // Flow supported by UseOpenIdConnectAuthentication (code + implicit)
                    RedirectUris = new List<string>
                    {
                        // Where to redirect the call, the MVC address
                        "https://localhost:44333" + "/signin-oidc" // default Microsoft openId middle ware callback path
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:44333"
                    },
                    AllowedScopes = new List<string>
                    {
                        // Information that the application can access,
                         StandardScopes.OpenId.Name,
                         StandardScopes.Email.Name,
                         StandardScopes.Profile.Name,
                         StandardScopes.OfflineAccess.Name,
                         "role",
                         "tickets_api"
                    },
                }
            };

            var factory = new IdentityServerServiceFactory();
            factory.UseInMemoryClients(clients);
            factory.UseInMemoryScopes(scopes);
            factory.UseInMemoryUsers(users);

            app.UseIdentityServer(new IdentityServerOptions
            {
                SiteName = "Identity Server Demo",
                SigningCertificate = SigningCertificate.Load(), // certificate used to sign in issued tokens
                Factory = factory // factory to find all users and resources
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Path not found: " + context.Request.Path);
            });
        }
    }
}
