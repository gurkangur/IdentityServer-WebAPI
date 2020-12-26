using Microsoft.AspNetCore.Identity;
using System;

namespace IdentityServer_WebAPI.Identity.Data.Entities
{
    public class ApplicationUserClaim : IdentityUserClaim<Guid>
    {
        public Guid OperationClaimId { get; set; }
        public OperationClaim OperationClaim { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
