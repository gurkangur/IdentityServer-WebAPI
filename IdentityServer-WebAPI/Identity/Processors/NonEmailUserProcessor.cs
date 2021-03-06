﻿using IdentityServer_WebAPI.Identity.Interfaces;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Processors
{
    public class NonEmailUserProcessor : INonEmailUserProcessor
    {
        private readonly IUserManager _userManager;
        public NonEmailUserProcessor(IUserManager userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public async Task<GrantValidationResult> ProcessAsync(JObject userInfo, string provider)
        {
            var userEmail = userInfo.Value<string>("email");

            if (provider.ToLower() == "linkedin")
                userEmail = userInfo.Value<string>("emailAddress");

            var userExternalId = userInfo.Value<string>("id");

            if (string.IsNullOrWhiteSpace(userEmail))
            {
                var existingUserId = await _userManager.FindByProviderAsync(provider, userExternalId);
                if (string.IsNullOrWhiteSpace(existingUserId))
                {
                    var customResponse = new Dictionary<string, object>
                    {
                        { "userInfo", userInfo }
                    };
                    return new GrantValidationResult(TokenRequestErrors.InvalidRequest,
                                                     "could not retrieve user's email from the given provider, include email paramater and send request again.",
                                                    customResponse);
                }
                else
                {
                    var userClaims = await _userManager.GetUserClaimsByExternalIdAsync(userExternalId, provider);
                    return new GrantValidationResult(existingUserId, provider, userClaims, provider, null);
                }

            }

            var newUserId = await _userManager.CreateExternalUserAsync(userExternalId, userEmail, provider);
            if (!string.IsNullOrWhiteSpace(newUserId))
            {
                var claims = await _userManager.GetUserClaimsByExternalIdAsync(userExternalId, provider);
                return new GrantValidationResult(newUserId, provider, claims, provider, null);
            }

            return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not create user, please try again.");

        }
    }
}
