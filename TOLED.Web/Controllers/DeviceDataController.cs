using Microsoft.AspNetCore.Mvc;
using TOLED.Web.Services;

namespace TOLED.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceDataController(IDeviceDataService _deviceDataService, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        [HttpGet("{deviceId}/active")]
        public async Task<IActionResult> GetActiveData([FromRoute]Guid deviceId)
        {
            var activeImage = await _deviceDataService.GetActiveImageForDeviceAsync(deviceId);

            if (activeImage?.DisplayData == null)
            {
                return NoContent();
            }

            httpContextAccessor.HttpContext?.Response.Headers.Append("Content-Length", activeImage.DisplayData?.Length.ToString());
            httpContextAccessor.HttpContext?.Response.Headers.Append("X-Frames", activeImage.Frames.ToString());
            httpContextAccessor.HttpContext?.Response.Headers.Append("X-ImageId", activeImage.Frames.ToString());

            return File(activeImage.DisplayData!, "application/octet-stream");
        }
    }
}
