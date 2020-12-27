using IdentityServer_WebAPI.Identity.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Interfaces
{
    public interface IUserManager
    {
        Task<List<Claim>> GetUserClaimsByIdAsync(string userId);
        Task<string> FindByIdAsync(string userId);
        Task<string> GetUserIdAsync(ApplicationUser user);
        Task<ApplicationUser> FindUserByIdAsync(string userId);
        Task<ApplicationUser> FindByNameAsync(string userName);
        Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure);
        Task<string> FindByProviderAsync(string provider, string externalId);
        Task<List<Claim>> GetUserClaimsByExternalIdAsync(string externalId, string provider);
        Task<string> CreateExternalUserAsync(string externalId, string email, string provider);
        Task<bool> FindUserByEmailAsync(string email);
        Task<IdentityResult> DeleteUserAsync(string userId);
        Task<string> CreateUserAsync(string userName, string password);
        Task<IList<string>> GetUserRoles(string userId);
    }
}
