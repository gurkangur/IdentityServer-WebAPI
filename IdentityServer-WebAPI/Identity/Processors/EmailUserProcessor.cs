using IdentityServer_WebAPI.Identity.Interfaces;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Processors
{
    public class EmailUserProcessor : IEmailUserProcessor
    {
        private readonly IUserManager _userStore;

        public EmailUserProcessor(IUserManager userStore)
        {
            _userStore = userStore;
        }
        public async Task<GrantValidationResult> ProcessAsync(JObject userInfo, string email, string provider)
        {
            var userEmail = email;
            var userExternalId = userInfo.Value<string>("id");

            if (string.IsNullOrWhiteSpace(userExternalId))
            {
                return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not retrieve user id from the token provided");
            }

            if (await _userStore.FindUserByEmailAsync(userEmail))
            {
                return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "User with specified email already exists");

            }

            var newUserId = await _userStore.CreateExternalUserAsync(userExternalId, userEmail, provider);
            if (!string.IsNullOrWhiteSpace(newUserId))
            {
                var claims = await _userStore.GetUserClaimsByExternalIdAsync(userExternalId, provider);
                return new GrantValidationResult(newUserId, provider, claims, provider, null);
            }
            return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not create user , please try again.");
        }
    }
}
