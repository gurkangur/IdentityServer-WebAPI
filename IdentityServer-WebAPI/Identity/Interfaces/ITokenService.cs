using IdentityServer_WebAPI.Identity.Models;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponse> GetToken(TokenRequest request);
    }
}
