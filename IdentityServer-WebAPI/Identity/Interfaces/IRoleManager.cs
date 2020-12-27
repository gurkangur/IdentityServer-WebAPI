using IdentityServer_WebAPI.Identity.Data.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Interfaces
{
    public interface IRoleManager
    {
        Task<ApplicationRole> CreateAsync(ApplicationRole applicationRole);
        Task<ApplicationRole> CreateWithOperationClaimsAsync(ApplicationRole applicationRole);
        Task<ApplicationRole> FindByIdAsync(Guid roleId);
        Task<ApplicationRole> UpdateAsync(ApplicationRole applicationRole);
        Task<ApplicationRole> UpdateWithOperationClaimsAsync(ApplicationRole applicationRole);
        Task<ApplicationRole> DeleteAsync(ApplicationRole applicationRole);
        Task<List<ApplicationRole>> GetApplicationRolesAsync();
        Task<List<Claim>> GetClaimsAsync(Guid roleId);
        Task<List<OperationClaim>> GetOperationClaimsAsync(Guid roleId);
    }
}
