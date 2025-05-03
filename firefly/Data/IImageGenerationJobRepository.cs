using firefly.Models;

namespace firefly.Data
{
    public interface IImageGenerationJobRepository
    {
        Task SaveJobAsync(ImageGenerationJob imageGenerationJob);
        Task<List<ImageGenerationJob>> GetPendingJobsAsync();
        Task SaveAssetAsync(ImageAsset imageAsset);
        Task MarkJobAsCompletedAsync(string jobId);
    }
}