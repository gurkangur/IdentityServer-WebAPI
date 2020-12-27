using IdentityServer_WebAPI.Identity.Grants;
using IdentityServer_WebAPI.Identity.Helpers;
using IdentityServer_WebAPI.Identity.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Providers
{
    public class TwitterAuthProvider : IExternalTokenProvider
    {
        private readonly HttpClient _client;
        public TwitterAuthProvider()
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

            var request = new Dictionary<string, string>();
            request.Add("tokenString", accessToken);
            request.Add("endpoint", provider.UserInfoEndpoint);

            var authorizationHeaderParams = QueryBuilder.GetQuery(request, TokenExchangeProviders.Twitter);

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            _client.DefaultRequestHeaders.Add("Authorization", authorizationHeaderParams);

            var result = await _client.GetAsync(provider.UserInfoEndpoint);

            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(await result.Content.ReadAsStringAsync());
                return infoObject;
            }
            return null;
        }
    }
}
