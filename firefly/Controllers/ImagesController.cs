using firefly.Data;
using firefly.Models.Requests;
using firefly.Models.Responses;
using firefly.Services;
using firefly.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost("upload/blob")]
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

        [HttpPost("generate-bulk")]
        public async Task<IActionResult> GenerateBulkAsync([FromBody] List<GenerateImageRequest> requests)
        {
            if (requests == null || !requests.Any())
                return BadRequest("At least one image generation request is required.");

            var results = await _imageService.GenerateBulkImagesAsync(requests);
            return Ok(results);
        }


        [HttpPost("expand-async")]
        public async Task<IActionResult> ExpandAsync([FromBody] ExpandImageRequest request)
        {
            var result = await _imageService.ExpandImageAsync(request);
            return Ok(result);
        }

        [HttpGet("/jobs/{jobId}/status")]
        public async Task<IActionResult> GetJobStatus(string jobId)
        {
            var isJobCompleted = await _dbContext.ImageGenerationJobs
                .AnyAsync(j => j.JobId == jobId && j.IsCompleted);

            return Ok(new { completed = isJobCompleted });
        }

        [HttpGet("result/{jobId}")]
        public async Task<IActionResult> GetJobResult(string jobId)
        {
            var result = await _imageService.GetJobResultAsync(jobId);
            return Ok(result);
        }

        [HttpGet("/jobs/{jobId}/images")]
        public async Task<IActionResult> GetImagesAsync(string jobId)
        {
            var sasUrls = new List<string>();
            
            var assetsJobIds = _dbContext.ImageAssets
                .Include(a => a.Job)
                .Where(j => j.Job.JobId == jobId)
                .ToList();
            
            if (assetsJobIds == null || assetsJobIds.Count == 0)
                return NotFound();
           
            foreach (var asset in assetsJobIds)
            {
                var sasUrl = await _blobService.GetSasUrl(asset.BlobPath, TimeSpan.FromHours(2));
                if(!String.IsNullOrEmpty(sasUrl))
                {
                    sasUrls.Add(sasUrl);
                }
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

                if (!String.IsNullOrEmpty(sasUrl))
                {                    
                    sasUrls.Add(sasUrl);
                }
            }
            return Ok(new { urls = sasUrls });
        }

        [HttpPost("upload/firefly")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            var result = await _imageService.UploadImageAsync(file);
            return Ok(result);
        }

    }
}
