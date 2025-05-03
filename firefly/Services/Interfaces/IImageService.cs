using firefly.Models;
using firefly.Models.Requests;
using firefly.Models.Responses;

namespace firefly.Services.Interfaces
{
    public interface IImageService
    {
        Task<ExpandImageResponse> ExpandImageAsync(ExpandImageRequest request);
        Task<GenerateImageResponse> GenerateImageAsync(GenerateImageRequest request);
        Task<JobResult> GetJobResultAsync(string jobId);
    }
}