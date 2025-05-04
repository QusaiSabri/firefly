using firefly.Data;
using firefly.Models.Requests;
using firefly.Models.Responses;
using firefly.Services;
using firefly.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace firefly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : Controller
    {
        private readonly FireflyStorageService _storageService;
        private readonly IImageService _imageService;
        private readonly IStorageService _blobService;
        private readonly LuminarDbContext _dbContext;

        public ImagesController(FireflyStorageService storageService, IImageService imageService, LuminarDbContext dbContext, IStorageService blobService)
        {
            _storageService = storageService;
            _imageService = imageService;
            _dbContext = dbContext;
            _blobService = blobService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            var uploadId = await _storageService.UploadImageAsync(file);
            return Ok(new UploadImageResponse { UploadId = uploadId });
        }
        
        [HttpPost("generate-async")]
        public async Task<IActionResult> GenerateAsync([FromBody] GenerateImageRequest request)
        {
            var result = await _imageService.GenerateImageAsync(request);
            return Ok(result);
        }

        [HttpPost("expand-async")]
        public async Task<IActionResult> ExpandAsync([FromBody] ExpandImageRequest request)
        {
            var result = await _imageService.ExpandImageAsync(request);
            return Ok(result);
        }


        [HttpGet("result/{jobId}")]
        public async Task<IActionResult> GetJobResult(string jobId)
        {
            var result = await _imageService.GetJobResultAsync(jobId);
            return Ok(result);
        }

        [HttpGet("/jobs/{jobId}/images")]
        public async Task<IActionResult> GetImagesAsync(Guid jobId)
        {
            var sasUrls = new List<string>();
            
            var assetsJobIds = _dbContext.ImageAssets.Where(a => a.JobId == jobId).ToList();
            if (assetsJobIds == null || assetsJobIds.Count == 0)
                return NotFound();
           
            foreach (var asset in assetsJobIds)
            {
                var sasUrl = await _blobService.GetSasUrl(asset.BlobPath, TimeSpan.FromHours(2));
                sasUrls.Add(sasUrl);
            }
            
            return Ok(new { urls = sasUrls });
        }

        // get all images for all existing jobs
        [HttpGet]
        public async Task<IActionResult> GetAllImagesAsync([FromQuery] int? limit)
        {
            var sasUrls = new List<string>();

            var assetsJobIds = _dbContext.ImageAssets
                .OrderByDescending(a => a.Id)
                .Take(limit ?? int.MaxValue)
                .ToList();

            if (assetsJobIds == null || assetsJobIds.Count == 0)
                return NotFound();

            foreach (var asset in assetsJobIds)
            {
                var sasUrl = await _blobService.GetSasUrl(asset.BlobPath, TimeSpan.FromHours(2));
                sasUrls.Add(sasUrl);
            }
            return Ok(new { urls = sasUrls });
        }
    }
}
