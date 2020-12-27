using IdentityServer_WebAPI.Identity.Data.Contexts;
using IdentityServer_WebAPI.Identity.Data.Entities;
using IdentityServer_WebAPI.Identity.Interfaces;
using IdentityServer_WebAPI.Identity.Services;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer_WebAPI.Identity.DependencyResolvers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCustomIdentityServer(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddScoped<IUserManager, UserManagerService>();
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseNpgsql(configuration.GetConnectionString("IdentityServerConnection")));

            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            

            if (environment.IsDevelopment())
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
                    {
                        options.Clients.Add(new Client
                        {
                            AllowOfflineAccess = true,
                            AccessTokenLifetime = 1800,
                            RefreshTokenExpiration = TokenExpiration.Absolute,
                            AbsoluteRefreshTokenLifetime = 1800,
                            AllowedScopes = {
                                IdentityServerConstants.StandardScopes.Profile,
                                IdentityServerConstants.StandardScopes.OpenId,
                                IdentityServerConstants.StandardScopes.OfflineAccess
                            },
                            LogoUri = null,
                            RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                            Enabled = true,
                            ClientId = "postman",
                            ClientSecrets = { new Secret("secret".Sha256()) },
                            ClientName = "PostMan Login",
                            PostLogoutRedirectUris = { "http://localhost:5000/signout-callback-oidc" },
                            ClientUri = null,
                            AllowedGrantTypes = { "password", "external" },
                            AllowAccessTokensViaBrowser = true,
                            AlwaysSendClientClaims = true,
                            AlwaysIncludeUserClaimsInIdToken = true,
                            UpdateAccessTokenClaimsOnRefresh = true,
                            RefreshTokenUsage = TokenUsage.OneTimeOnly

                        });
                    })
                    .AddCustomProfileService()
                    .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>()
                    .AddTokenExchangeForExternalProviders()
                    .AddExternalTokenProviders();
            }
            else
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>()
                    .AddCustomProfileService()
                    .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>()
                    .AddTokenExchangeForExternalProviders()
                    .AddExternalTokenProviders();
            }
            services.AddTransient<IProfileService, CustomProfileService>();
            services.AddAuthentication()
                .AddIdentityServerJwt();
            return services;
        }
    }
}
