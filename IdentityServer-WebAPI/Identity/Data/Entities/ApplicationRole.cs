using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IdentityServer_WebAPI.Identity.Data.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ICollection<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }

    }
}
