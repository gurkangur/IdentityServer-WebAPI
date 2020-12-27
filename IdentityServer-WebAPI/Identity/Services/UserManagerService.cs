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
    public class UserManagerService : IUserManager
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserManagerService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, true);
        }

        public async Task<string> CreateExternalUserAsync(string externalId, string email, string provider)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, externalId, provider));
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
            }
            else
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, externalId, provider));
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                }
            }
            return user?.Id.ToString();
        }

        public async Task<string> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);
            return user.Id.ToString();
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == Guid.Parse(userId));

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<string> FindByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            return user.Id.ToString();
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<string> FindByProviderAsync(string provider, string externalId)
        {
            var user = await _userManager.FindByLoginAsync(provider, externalId);
            return user?.Id.ToString();
        }

        public async Task<bool> FindUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<ApplicationUser> FindUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<List<Claim>> GetUserClaimsByExternalIdAsync(string externalId, string provider)
        {
            var user = await _userManager.FindByLoginAsync(provider, externalId);

            if (user != null)
            {
                var claims = await GetUserClaimsByIdAsync(user.Id.ToString());
                return claims.ToList();
            }

            return new List<Claim>();
        }

        public async Task<List<Claim>> GetUserClaimsByIdAsync(string userId)
        {
            return await (from userClaim in _applicationDbContext.UserClaims
                          join operationClaim in _applicationDbContext.OperationClaims on userClaim.OperationClaimId equals operationClaim.Id
                          where userClaim.UserId == Guid.Parse(userId)
                          select new Claim(ClaimTypes.Role, operationClaim.Name)).ToListAsync();
        }
        public async Task<IList<string>> GetUserRoles(string userId)
        {
            var user = await FindUserByIdAsync(userId);
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<string> GetUserIdAsync(ApplicationUser user)
        {
            return await _userManager.GetUserIdAsync(user);
        }

        public async Task<List<OperationClaim>> GetUserOperationClaimsByIdAsync(Guid id)
        {
            return await (from userClaim in _applicationDbContext.UserClaims
                          join operationClaim in _applicationDbContext.OperationClaims on userClaim.OperationClaimId equals operationClaim.Id
                          where userClaim.UserId == id
                          select operationClaim).ToListAsync();
        }

        public async Task<ApplicationUser> GetUserWithOperationClaims(Guid id)
        {
            var user = await _applicationDbContext.Users.SingleOrDefaultAsync(x => x.Id == id);
            user.ApplicationUserClaims = _applicationDbContext.UserClaims.Where(x => x.UserId == user.Id).ToList();
            user.ApplicationUserClaims.ToList().ForEach(userclaim =>
            {
                userclaim.OperationClaim = _applicationDbContext.OperationClaims.SingleOrDefault(x => x.Id == userclaim.OperationClaimId);
            });
            return user;
        }
    }
}
