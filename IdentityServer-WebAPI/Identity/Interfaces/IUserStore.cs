using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Interfaces
{
    public interface IUserStore
    {
        Task<List<Claim>> GetUserClaimsByIdAsync(string userId);
        Task<string> FindByIdAsync(string userId);
    }
}
