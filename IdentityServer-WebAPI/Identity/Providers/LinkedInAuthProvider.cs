using IdentityServer_WebAPI.Identity.Grants;
using IdentityServer_WebAPI.Identity.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Providers
{
    public class LinkedInAuthProvider : IExternalTokenProvider
    {
        private readonly HttpClient _client;
        public LinkedInAuthProvider()
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

            var urlParams = $"oauth2_access_token={accessToken}&format=json";

            var result = await _client.GetAsync($"{provider.UserInfoEndpoint}{urlParams}");
            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(await result.Content.ReadAsStringAsync());
                return infoObject;
            }
            return null;
        }
    }
}
