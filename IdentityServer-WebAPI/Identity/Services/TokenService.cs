using IdentityModel;
using IdentityServer_WebAPI.Identity.Interfaces;
using IdentityServer_WebAPI.Identity.Models;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using IdpTokenResponse = IdentityServer4.ResponseHandling.TokenResponse;

namespace IdentityServer_WebAPI.Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRequestValidator _requestValidator;
        private readonly IClientSecretValidator _clientValidator;
        private readonly ITokenResponseGenerator _responseGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(
          ITokenRequestValidator requestValidator,
          IClientSecretValidator clientValidator,
          ITokenResponseGenerator responseGenerator,
          IHttpContextAccessor httpContextAccessor)
        {
            _requestValidator = requestValidator;
            _clientValidator = clientValidator;
            _responseGenerator = responseGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Models.TokenResponse> GetToken(TokenRequest request)
        {
            var parameters = new NameValueCollection
      {
        { "username", request.Username },
        { "password", request.Password },
        { "grant_type", request.GrantType },
        { "scope", request.Scope },
        { "refresh_token", request.RefreshToken },
        { "response_type", OidcConstants.ResponseTypes.Token }
      };

            var response = await GetIdpToken(parameters);

            return GetTokenResponse(response);
        }

        private async Task<IdpTokenResponse> GetIdpToken(NameValueCollection parameters)
        {
            var clientResult = await _clientValidator.ValidateAsync(_httpContextAccessor.HttpContext);

            if (clientResult.IsError)
            {
                return new IdpTokenResponse
                {
                    Custom = new Dictionary<string, object>
          {
            { "Error", "invalid_client" },
            { "ErrorDescription", "Invalid client/secret combination" }
          }
                };
            }

            var validationResult = await _requestValidator.ValidateRequestAsync(parameters, clientResult);

            if (validationResult.IsError)
            {
                return new IdpTokenResponse
                {
                    Custom = new Dictionary<string, object>
          {
            { "Error", validationResult.Error },
            { "ErrorDescription", validationResult.ErrorDescription }
          }
                };
            }

            return await _responseGenerator.ProcessAsync(validationResult);
        }

        private static Models.TokenResponse GetTokenResponse(IdpTokenResponse response)
        {
            if (response.Custom != null && response.Custom.ContainsKey("Error"))
            {
                return new Models.TokenResponse
                {
                    Error = response.Custom["Error"].ToString(),
                    ErrorDescription = response.Custom["ErrorDescription"]?.ToString()
                };
            }

            return new Models.TokenResponse
            {
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken,
                ExpiresIn = response.AccessTokenLifetime,
                TokenType = "Bearer"
            };
        }
    }
}
