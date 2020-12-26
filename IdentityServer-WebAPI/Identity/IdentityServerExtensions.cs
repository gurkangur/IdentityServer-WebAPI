using IdentityServer_WebAPI.Identity.Grants;
using IdentityServer_WebAPI.Identity.Interfaces;
using IdentityServer_WebAPI.Identity.Processors;
using IdentityServer_WebAPI.Identity.Providers;
using IdentityServer_WebAPI.Identity.Services;
using IdentityServer_WebAPI.Identity.Stores;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IdentityServer_WebAPI.Identity
{
    public static class IdentityServerExtensions
    {
        public static IIdentityServerBuilder AddUserStore(this IIdentityServerBuilder services)
        {
            services.Services.AddScoped<IUserStore, UserStore>();
            return services;
        }
        public static IIdentityServerBuilder AddCustomProfileService(this IIdentityServerBuilder services)
        {
            services.AddProfileService<CustomProfileService>();
            return services;
        }
        public static IIdentityServerBuilder AddTokenExchangeForExternalProviders(this IIdentityServerBuilder services)
        {
            services.AddExtensionGrantValidator<ExternalAuthenticationGrant>();
            services.Services.AddScoped<INonEmailUserProcessor, NonEmailUserProcessor>();
            services.Services.AddScoped<IEmailUserProcessor, EmailUserProcessor>();
            return services;
        }
        public static IIdentityServerBuilder AddExternalTokenProviders(this IIdentityServerBuilder services)
        {
            services.Services.AddScoped<FacebookAuthProvider>();
            services.Services.AddScoped<Func<string, IExternalTokenProvider>>(serviceProvider => key =>
            {
                var name = $"{key}AuthProvider";

                Assembly entryAssembly = Assembly.GetEntryAssembly();
                Assembly externalTokenExchangeAssembly = typeof(FacebookAuthProvider).Assembly;
                List<Assembly> assemblies = new List<Assembly>() { externalTokenExchangeAssembly, entryAssembly };

                Type authProviderType = null;
                foreach (Assembly assembly in assemblies)
                {
                    authProviderType = assembly.ExportedTypes.FirstOrDefault(x => String.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));
                    if (authProviderType != null) break;
                }

                if (authProviderType == null)
                    throw new Exception("Provider not found");

                return (IExternalTokenProvider)serviceProvider.GetService(authProviderType);
            });
            return services;
        }
    }
}
