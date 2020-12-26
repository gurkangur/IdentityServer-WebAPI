﻿using IdentityServer_WebAPI.Identity.Grants;
using IdentityServer_WebAPI.Identity.Helpers;
using IdentityServer_WebAPI.Identity.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Providers
{
    public class FacebookAuthProvider : IExternalTokenProvider
    {
        private readonly HttpClient _client;

        public FacebookAuthProvider()
        {
            _client = new HttpClient();
        }
        public async Task<JObject> GetUserInfoAsync(string accessToken)
        {
            var provider = ExternalProviders.GetProviders().FirstOrDefault(x => x.Name.Equals(TokenExchangeProviders.Facebook, StringComparison.OrdinalIgnoreCase));
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            var request = new Dictionary<string, string>
            {
                {"fields", provider.Fields},
                {"access_token", accessToken}
            };

            var result = await _client.GetAsync(provider.UserInfoEndpoint + QueryBuilder.GetQuery(request, TokenExchangeProviders.Facebook));
            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(await result.Content.ReadAsStringAsync());
                return infoObject;
            }
            return null;
        }
    }
}
