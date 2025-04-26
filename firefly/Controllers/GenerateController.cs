using firefly.Models;
using firefly.Services;
using Microsoft.AspNetCore.Mvc;

namespace firefly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenerateController : Controller
    {
        private readonly FireflyStorageService _storageService;
        private readonly FireflyImageService _imageService;

        public GenerateController(FireflyStorageService storageService, FireflyImageService imageService)
        {
            _storageService = storageService;
            _imageService = imageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            var uploadId = await _storageService.UploadImageAsync(file);
            return Ok(new UploadImageResponse { UploadId = uploadId });
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] GenerateImageRequest request)
        {
            var imageUrl = await _imageService.GenerateImageAsync(request.Prompt, request.ReferenceImageId);
            return Ok(new FireflyImageResponse { GeneratedImageUrl = imageUrl });
        }
    }
}
