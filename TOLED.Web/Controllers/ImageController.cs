using Microsoft.AspNetCore.Mvc;
using TOLED.Web.Services;

namespace TOLED.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController(IImageService _imageService, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        [HttpGet("data/{id}")]
        public async Task<IActionResult> GetImageData([FromRoute] int id)
        {
            var processedImage = await _imageService.GetImageAsync(id);

            if (processedImage?.DisplayData == null)
            {
                return NoContent();
            }

            return File(processedImage.DisplayData, "application/octet-stream");
        }

        [HttpGet("data/active")]
        public async Task<IActionResult> GetActiveImageData()
        {
            var processedImage = await _imageService.GetActiveImageAsync();

            if (processedImage?.DisplayData == null)
            {
                return NoContent();
            }

            httpContextAccessor.HttpContext?.Response.Headers.Append("Content-Length", processedImage.DisplayData?.Length.ToString());
            httpContextAccessor.HttpContext?.Response.Headers.Append("Frames", processedImage.Frames.ToString());

            return File(processedImage.DisplayData!, "application/octet-stream");
        }
    }
}
