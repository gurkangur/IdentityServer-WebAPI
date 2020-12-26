using IdentityServer_WebAPI.Identity.Interfaces;
using IdentityServer_WebAPI.Identity.Services;
using IdentityServer_WebAPI.Identity.Stores;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
