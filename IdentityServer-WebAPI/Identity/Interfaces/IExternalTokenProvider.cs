using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Interfaces
{
    public interface IExternalTokenProvider
    {
        Task<JObject> GetUserInfoAsync(string accessToken);
    }
}
