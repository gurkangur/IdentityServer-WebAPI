using IdentityServer_WebAPI.Identity.Data.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace IdentityServer_WebAPI.Identity.Data.Contexts
{
    public class ApplicationDbContext : KeyApiAuthorizationDbContext<ApplicationUser, ApplicationRole, Guid, 
        ApplicationUserClaim, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, ApplicationRoleClaim, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public DbSet<OperationClaim> OperationClaims { get; set; }
    }
}
