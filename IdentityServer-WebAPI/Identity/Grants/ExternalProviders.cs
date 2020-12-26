using IdentityServer_WebAPI.Identity.Models;
using System.Collections.Generic;

namespace IdentityServer_WebAPI.Identity.Grants
{
    public static class ExternalProviders
    {
        public static IEnumerable<ExternalProvider> GetProviders()
        {
            return new List<ExternalProvider>
            {
                new ExternalProvider
                {
                    Id = 1,
                    Fields = "id,email,name,gender,birthday",
                    Name = "Facebook",
                    UserInfoEndpoint = "https://graph.facebook.com/v2.8/me"
                },
                new ExternalProvider
                {
                    Id = 2,
                    Name = "Google",
                    UserInfoEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo"
                },
                new ExternalProvider
                {
                    Id = 3,
                    Name = "Twitter",
                    UserInfoEndpoint = "https://api.twitter.com/1.1/account/verify_credentials.json"
                }

            };
        }
    }
}
