using IdentityServer_WebAPI.Identity.Interfaces;
using IdentityServer_WebAPI.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<ActionResult<TokenResponse>> Post([FromForm] TokenRequest request)
        {
            var response = await _tokenService.GetToken(request);

            if (!string.IsNullOrEmpty(response.Error))
            {
                return new BadRequestObjectResult(response);
            }

            return response;
        }
    }
}
