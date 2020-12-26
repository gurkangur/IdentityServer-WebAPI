using IdentityServer_WebAPI.Identity.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Stores
{
    public class UserManagerService : IUserManager
    {
        public async Task<string> CreateExternalUserAsync(string externalId, string email, string provider)
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<string> FindByIdAsync(string userId)
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<string> FindByProviderAsync(string provider, string externalId)
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<bool> FindUserByEmailAsync(string email)
        {
            return false;
        }

        public async Task<List<Claim>> GetUserClaimsByExternalIdAsync(string externalId, string provider)
        {
            return new List<Claim>();
        }

        public async Task<List<Claim>> GetUserClaimsByIdAsync(string userId)
        {
            return new List<Claim>();
        }
    }
}
