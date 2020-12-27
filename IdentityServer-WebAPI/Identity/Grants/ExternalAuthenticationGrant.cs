using IdentityServer_WebAPI.Identity.Interfaces;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Grants
{
    public class ExternalAuthenticationGrant : IExtensionGrantValidator
    {
        public string GrantType => "external";

        private readonly Func<string, IExternalTokenProvider> _tokenServiceAccessor;
        private readonly IUserManager _userManager;
        private readonly IEmailUserProcessor _emailUserProcessor;
        private readonly INonEmailUserProcessor _nonEmailUserProcessor;

        public ExternalAuthenticationGrant(
            Func<string, IExternalTokenProvider> tokenServiceAccessor,
            IUserManager userManager,
            IEmailUserProcessor emailUserProcessor,
            INonEmailUserProcessor nonEmailUserProcessor
          )
        {
            _tokenServiceAccessor = tokenServiceAccessor ?? throw new ArgumentNullException(nameof(tokenServiceAccessor));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _emailUserProcessor = emailUserProcessor ?? throw new ArgumentNullException(nameof(emailUserProcessor));
            _nonEmailUserProcessor = nonEmailUserProcessor ?? throw new ArgumentNullException(nameof(nonEmailUserProcessor));
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            try
            {
                var providerName = context.Request.Raw.Get(ExternalGrantParameters.Provider);
                var externalToken = context.Request.Raw.Get(ExternalGrantParameters.ExternalToken);
                var requestEmail = context.Request.Raw.Get(ExternalGrantParameters.Email);

                if (string.IsNullOrWhiteSpace(providerName))
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, ExternalGrantErrors.InvalidProvider);
                    return;
                }

                if (string.IsNullOrWhiteSpace(externalToken))
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, ExternalGrantErrors.InvalidExternalToken);
                    return;
                }

                var provider = ExternalProviders.GetProviders().FirstOrDefault(x => x.Name.Equals(providerName, StringComparison.OrdinalIgnoreCase));
                if (provider == null)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, ExternalGrantErrors.ProviderNotFound);
                    return;
                }

                var tokenService = _tokenServiceAccessor(providerName);
                var userInfo = await tokenService.GetUserInfoAsync(externalToken);
                if (userInfo == null)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, ExternalGrantErrors.UserInfoNotRetrieved);
                    return;
                }

                var externalId = userInfo.Value<string>("id");
                if (!string.IsNullOrWhiteSpace(externalId))
                {
                    var existingUserId = await _userManager.FindByProviderAsync(providerName, externalId);
                    if (!string.IsNullOrWhiteSpace(existingUserId))
                    {
                        var claims = await _userManager.GetUserClaimsByExternalIdAsync(externalId, providerName);
                        context.Result = new GrantValidationResult(existingUserId, providerName, claims, providerName, null);
                    }
                }

                if (string.IsNullOrWhiteSpace(requestEmail))
                {
                    context.Result = await _nonEmailUserProcessor.ProcessAsync(userInfo, providerName);
                    return;
                }

                context.Result = await _emailUserProcessor.ProcessAsync(userInfo, requestEmail, providerName);
            }
            catch (Exception e)
            {

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, e.Message);
            }
        }
    }
}
