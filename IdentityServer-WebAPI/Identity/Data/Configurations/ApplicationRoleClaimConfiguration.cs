using IdentityServer_WebAPI.Identity.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer_WebAPI.Identity.Data.Configurations
{
    public class ApplicationRoleClaimConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            builder.Ignore(p => p.ClaimType);
            builder.Ignore(p => p.ClaimValue);


            builder.HasKey(bc => new { bc.RoleId, bc.OperationClaimId });

            builder.HasOne(bc => bc.ApplicationRole)
                .WithMany(b => b.ApplicationRoleClaims)
                .HasForeignKey(bc => bc.RoleId);


            builder.HasOne(bc => bc.OperationClaim)
               .WithMany(c => c.ApplicationRoleClaims)
               .HasForeignKey(bc => bc.OperationClaimId);


        }
    }
}
