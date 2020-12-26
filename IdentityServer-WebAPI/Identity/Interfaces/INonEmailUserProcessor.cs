using IdentityServer4.Validation;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Identity.Interfaces
{
    public interface INonEmailUserProcessor
    {
        Task<GrantValidationResult> ProcessAsync(JObject userInfo, string provider);
    }
}
