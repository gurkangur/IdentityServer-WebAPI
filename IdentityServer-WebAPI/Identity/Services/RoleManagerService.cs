using IdentityServer_WebAPI.Identity.Data.Contexts;
using IdentityServer_WebAPI.Identity.Data.Entities;
using IdentityServer_WebAPI.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Services
{
    public class RoleManagerService : IRoleManager
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleManagerService(ApplicationDbContext applicationDbContext, RoleManager<ApplicationRole> roleManager)
        {
            _applicationDbContext = applicationDbContext;
            _roleManager = roleManager;
        }

        public async Task<ApplicationRole> CreateAsync(ApplicationRole applicationRole)
        {
            await _roleManager.CreateAsync(applicationRole);
            return applicationRole;
        }

        public async Task<ApplicationRole> CreateWithOperationClaimsAsync(ApplicationRole applicationRole)
        {
            _applicationDbContext.Roles.Add(applicationRole);
            _applicationDbContext.RoleClaims.AddRange(applicationRole.ApplicationRoleClaims);
            await _applicationDbContext.SaveChangesAsync();
            return applicationRole;
        }

        public async Task<ApplicationRole> DeleteAsync(ApplicationRole applicationRole)
        {
            await _roleManager.DeleteAsync(applicationRole);
            return applicationRole;
        }

        public async Task<ApplicationRole> FindByIdAsync(Guid roleId)
        {
            return await _roleManager.FindByIdAsync(roleId.ToString());
        }

        public async Task<List<ApplicationRole>> GetApplicationRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<List<Claim>> GetClaimsAsync(Guid roleId)
        {
            return await (from roleClaim in _applicationDbContext.RoleClaims
                          join operationClaim in _applicationDbContext.OperationClaims on roleClaim.OperationClaimId equals operationClaim.Id
                          where roleClaim.RoleId == roleId
                          select new Claim(ClaimTypes.Role, operationClaim.Name)).ToListAsync();
        }

        public async Task<List<OperationClaim>> GetOperationClaimsAsync(Guid roleId)
        {
            return await (from roleClaim in _applicationDbContext.RoleClaims
                          join operationClaim in _applicationDbContext.OperationClaims on roleClaim.OperationClaimId equals operationClaim.Id
                          where roleClaim.RoleId == roleId
                          select operationClaim).ToListAsync();
        }

        public async Task<ApplicationRole> UpdateAsync(ApplicationRole applicationRole)
        {
            await _roleManager.UpdateAsync(applicationRole);
            return applicationRole;
        }

        public async Task<ApplicationRole> UpdateWithOperationClaimsAsync(ApplicationRole applicationRole)
        {
            await UpdateAsync(applicationRole);
            var claims = await _applicationDbContext.RoleClaims.Where(x => x.RoleId == applicationRole.Id).ToListAsync();
            _applicationDbContext.RoleClaims.RemoveRange(claims);
            _applicationDbContext.RoleClaims.AddRange(applicationRole.ApplicationRoleClaims);
            await _applicationDbContext.SaveChangesAsync();
            return applicationRole;
        }
    }
}
