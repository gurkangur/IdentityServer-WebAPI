using IdentityServer_WebAPI.Identity.Data.Entities;
using IdentityServer_WebAPI.Identity.Interfaces;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly IUserManager _userManager;

        public CustomProfileService(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var claims = await _userManager.GetUserClaimsByIdAsync(userId);
                context.IssuedClaims.AddRange(claims);
            }

        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindUserByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
