using Microsoft.AspNetCore.Identity;
using System;

namespace IdentityServer_WebAPI.Identity.Data.Entities
{
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
    {
        public Guid OperationClaimId { get; set; }
        public OperationClaim OperationClaim { get; set; }
        public ApplicationRole ApplicationRole { get; set; }
    }
}
