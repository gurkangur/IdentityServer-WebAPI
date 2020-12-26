﻿using IdentityServer_WebAPI.Identity.Interfaces;
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

        public ExternalAuthenticationGrant(
          Func<string, IExternalTokenProvider> tokenServiceAccessor

          )
        {
            _tokenServiceAccessor = tokenServiceAccessor ?? throw new ArgumentNullException(nameof(tokenServiceAccessor));
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
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


        }
    }
}
