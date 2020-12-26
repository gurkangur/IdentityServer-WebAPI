using IdentityServer_WebAPI.Identity.Interfaces;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly IUserStore _userStore;

        public CustomProfileService(IUserStore userStore)
        {
            _userStore = userStore;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = await _userStore.FindByIdAsync(context.Subject.GetSubjectId());
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var claims = await _userStore.GetUserClaimsByIdAsync(userId);
                context.IssuedClaims.AddRange(claims);
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectid = await _userStore.FindByIdAsync(context.Subject.GetSubjectId());
            context.IsActive = !string.IsNullOrWhiteSpace(subjectid);
        }
    }
}
