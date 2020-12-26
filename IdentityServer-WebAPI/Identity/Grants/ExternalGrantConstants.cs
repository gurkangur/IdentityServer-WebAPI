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
    }
}
