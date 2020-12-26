using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Data.Contexts
{
    public class KeyApiAuthorizationDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> : 
        IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>, IPersistedGrantDbContext
           where TUser : IdentityUser<TKey>
           where TRole : IdentityRole<TKey>
           where TKey : IEquatable<TKey>
           where TUserClaim : IdentityUserClaim<TKey>
           where TUserRole : IdentityUserRole<TKey>
           where TUserLogin : IdentityUserLogin<TKey>
           where TRoleClaim : IdentityRoleClaim<TKey>
           where TUserToken : IdentityUserToken<TKey>
    {
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;


        public KeyApiAuthorizationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options)
        {
            _operationalStoreOptions = operationalStoreOptions;
        }


        public DbSet<PersistedGrant> PersistedGrants { get; set; }


        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        Task<int> IPersistedGrantDbContext.SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);
        }
    }
}
