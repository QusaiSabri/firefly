using firefly.Models;
using Microsoft.EntityFrameworkCore;

namespace firefly.Data
{
    public class ImageGenerationJobRepository : IImageGenerationJobRepository
    {
        private readonly LuminarDbContext _dbContext;

        public ImageGenerationJobRepository(LuminarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveJobAsync(ImageGenerationJob imageGenerationJob)
        {
            _dbContext.ImageGenerationJobs.Add(imageGenerationJob);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ImageGenerationJob>> GetPendingJobsAsync()
        {
            return await _dbContext.ImageGenerationJobs
                .Where(job => !job.IsCompleted)
                .ToListAsync();
        }

        public async Task MarkJobAsCompletedAsync(string jobId)
        {
            var job = await _dbContext.ImageGenerationJobs
                        .FirstOrDefaultAsync(j => j.JobId == jobId);

            if (job != null)
            {
                job.IsCompleted = true;
                job.CompletedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task SaveAssetAsync(ImageAsset imageAsset)
        {
            _dbContext.ImageAssets.Add(imageAsset);
            await _dbContext.SaveChangesAsync();
        }
    }
}
