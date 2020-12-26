using IdentityServer_WebAPI.Identity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Stores
{
    public class UserStore : IUserStore
    {
        public async Task<string> FindByIdAsync(string userId)
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<List<Claim>> GetUserClaimsByIdAsync(string userId)
        {
            return new List<Claim>();
        }
    }
}
