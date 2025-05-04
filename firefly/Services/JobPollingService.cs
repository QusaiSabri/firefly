using firefly.Data;
using firefly.Models;
using firefly.Services.Interfaces;

namespace firefly.Services
{
    public class JobPollingService : BackgroundService
    {
        private readonly ILogger<JobPollingService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _pollInterval = TimeSpan.FromSeconds(5);
        private readonly IConfiguration _config;

        public JobPollingService(ILogger<JobPollingService> logger, IServiceScopeFactory scopeFactory, IConfiguration config)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Polling Service Started");
            var blobContainerName = _config["AzureBlob:ContainerName"];

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var jobRepo = scope.ServiceProvider.GetRequiredService<IImageGenerationJobRepository>();
                    var fireflyService = scope.ServiceProvider.GetRequiredService<IImageService>();
                    var storageService = scope.ServiceProvider.GetRequiredService<IStorageService>();

                    var pendingJobs = await jobRepo.GetPendingJobsAsync();

                    foreach (var job in pendingJobs)
                    {
                        var result = await fireflyService.GetJobResultAsync(job.JobId);

                        if (result?.Status == "succeeded" && result.Result?.Outputs?.Any() == true)
                        {
                            foreach (var output in result.Result.Outputs)
                            {

                                //upload to azure blob
                                var fileName = $"{output.Seed}_{Guid.NewGuid()}.jpg";
                                var azureUrl = await storageService.UploadFromUrlAsync(output.Image.Url, fileName);

                                var image = new ImageAsset
                               {
                                   JobId = job.Id,
                                   Url = azureUrl,
                                   Seed = output.Seed,
                                   Width = result.Result.Size.Width,
                                   Height = result.Result.Size.Height,
                                   ContentClass = result.Result.ContentClass,
                                   Prompt = job.Prompt,
                                   BlobPath = fileName

                                };

                                await jobRepo.SaveAssetAsync(image);
                            }

                            await jobRepo.MarkJobAsCompletedAsync(job.JobId);
                            _logger.LogInformation($"Processed and completed job {job.JobId}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during job polling");
                }

                await Task.Delay(_pollInterval, stoppingToken);
            }
        }
    }
}
