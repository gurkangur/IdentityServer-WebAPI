using IdentityServer_WebAPI.Identity.Interfaces;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace IdentityServer_WebAPI.Identity.Services
{
    public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserManager _userManager;

        public CustomResourceOwnerPasswordValidator(
            IUserManager userManager)
        {
            _userManager = userManager;
        }

        public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByNameAsync(context.UserName);
            if (user != null)
            {
                var result = await _userManager.CheckPasswordSignInAsync(user, context.Password, true);
                if (result.Succeeded)
                {
                    var sub = await _userManager.GetUserIdAsync(user);
                    context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password);
                    return;
                }
            }
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }
    }
}
