using firefly.Models;
using firefly.Services;
using firefly.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace firefly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenerateController : Controller
    {
        private readonly FireflyStorageService _storageService;
        //private readonly FireflyImageService _imageService;
        private readonly IImageGenerationService _imageService;

        public GenerateController(FireflyStorageService storageService, IImageGenerationService imageService)
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

        //[HttpPost("generate")]
        //public async Task<IActionResult> Generate([FromBody] GenerateImageRequest request)
        //{
        //    var imageUrl = await _imageService.GenerateImageAsync(request.Prompt, request.ReferenceImageId);
        //    return Ok(new FireflyImageResponse { GeneratedImageUrl = imageUrl });
        //}

        
        [HttpPost("generate-async")]
        public async Task<IActionResult> GenerateAsync([FromBody] GenerateImageRequest request)
        {
            var result = await _imageService.GenerateImageAsync(request);
            return Ok(result);
        }

        [HttpGet("result/{jobId}")]
        public async Task<IActionResult> GetJobResult(string jobId)
        {
            var result = await _imageService.GetJobResultAsync(jobId);
            return Ok(result);
        }
    }
}
