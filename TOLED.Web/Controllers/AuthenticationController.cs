using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace TOLED.Web.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost("signin-google")]
        public async Task<IActionResult> GoogleCallback(string credentials)
        {
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(credentials);
            return Ok();
        }
    }
}
