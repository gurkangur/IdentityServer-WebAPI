using IdentityServer_WebAPI.Identity.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer_WebAPI.Identity.Data.Configurations
{
    public class ApplicationUserClaimConfiguration : IEntityTypeConfiguration<ApplicationUserClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
        {
            builder.Ignore(p => p.ClaimType);
            builder.Ignore(p => p.ClaimValue);


            builder.HasKey(bc => new { bc.UserId, bc.OperationClaimId });

            builder.HasOne(bc => bc.ApplicationUser)
                .WithMany(b => b.ApplicationUserClaims)
                .HasForeignKey(bc => bc.UserId);


            builder.HasOne(bc => bc.OperationClaim)
               .WithMany(c => c.ApplicationUserClaims)
               .HasForeignKey(bc => bc.OperationClaimId);
        }
    }
}
