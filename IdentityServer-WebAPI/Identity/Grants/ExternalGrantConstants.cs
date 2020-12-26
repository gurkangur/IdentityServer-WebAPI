using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Grants
{
    public static class ExternalGrantParameters
    {
        public const string Provider = "provider";
        public const string ExternalToken = "external_token";
        public const string Email = "email";
    }

    public static class ExternalGrantErrors
    {
        public const string InvalidProvider = "invalid provider";
        public const string InvalidExternalToken = "invalid external token";
        public const string ProviderNotFound = "provider not found";
        public const string UserInfoNotRetrieved = "user info could not be retrieved from the given provider.";
    }
    public static class TokenExchangeProviders
    {
        public const string Facebook = "facebook";
        public const string LinkedIn = "linkedin";
        public const string Twitter = "twitter";
        public const string Google = "google";
        public const string GrantName = "external";
    }
}
