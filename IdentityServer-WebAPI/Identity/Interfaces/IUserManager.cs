using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Interfaces
{
    public interface IUserManager
    {
        Task<List<Claim>> GetUserClaimsByIdAsync(string userId);
        Task<string> FindByIdAsync(string userId);
        Task<string> FindByProviderAsync(string provider, string externalId);
        Task<List<Claim>> GetUserClaimsByExternalIdAsync(string externalId, string provider);
        Task<string> CreateExternalUserAsync(string externalId, string email, string provider);
        Task<bool> FindUserByEmailAsync(string email);
    }
}
