using Newtonsoft.Json;

namespace IdentityServer_WebAPI.Identity.Models
{
    public class TokenResponse
    {
        [JsonProperty("access_token", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string RefreshToken { get; set; }
        [JsonProperty("token_type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string TokenType { get; set; }
        [JsonProperty("expires_in", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ExpiresIn { get; set; }
        [JsonProperty("error", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Error { get; set; }
        [JsonProperty("error_description", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ErrorDescription { get; set; }
    }
}
