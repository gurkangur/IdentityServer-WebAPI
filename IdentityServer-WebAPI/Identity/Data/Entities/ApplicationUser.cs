using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IdentityServer_WebAPI.Identity.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<ApplicationUserClaim> ApplicationUserClaims { get; set; }
    }
}
