using firefly.Models;

namespace firefly.Services.Interfaces
{
    public interface IImageGenerationService
    {
        Task<GenerateImageResponse> GenerateImageAsync(GenerateImageRequest request);
        Task<JobResult> GetJobResultAsync(string jobId);
    }
}