using System;
using System.Collections.Generic;

namespace IdentityServer_WebAPI.Identity.Data.Entities
{
    public class OperationClaim
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }
        public ICollection<ApplicationUserClaim> ApplicationUserClaims { get; set; }
    }
}
