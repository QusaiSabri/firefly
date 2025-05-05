using firefly.Models;
using firefly.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using firefly.Data;
using firefly.Models.Requests;
using firefly.Models.Responses;
using System.Text.Json.Serialization;

namespace firefly.Services.Adapters
{
    public class FireflyAdapter : IImageService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly AuthService _authService;
        private readonly ILogger<FireflyAdapter> _logger;
        private readonly IImageGenerationJobRepository _jobRepo;

        public FireflyAdapter(HttpClient httpClient, IConfiguration config, ILogger<FireflyAdapter> logger, AuthService authService, IImageGenerationJobRepository jobRepo)
        {
            _httpClient = httpClient;
            _config = config;
            _authService = authService;
            _jobRepo = jobRepo;
            _logger = logger;
        }
        public async Task<GenerateImageResponse> GenerateImageAsync(GenerateImageRequest request)
        {
            var baseUrl = _config["Adobe:GenerateAsyncEndpoint"];
            var clientId = _config["Adobe:ClientId"];
            var token = await _authService.GetAccessTokenAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var jsonBody = JsonSerializer.Serialize(request, options);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, baseUrl);
            httpRequest.Headers.Add("x-api-key", clientId);
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();


            var result = JsonSerializer.Deserialize<GenerateImageResponse>(json);

            if(result == null || string.IsNullOrWhiteSpace(result.JobId))
            {
                _logger.LogWarning("Received null or invalid job result from Firefly.");
                throw new Exception("Invalid response from Firefly.");                
            }

            var job = new ImageGenerationJob
            {
                JobId = result.JobId!,
                Prompt = request.Prompt,
                StatusUrl = result.StatusUrl!,
                CancelUrl = result.CancelUrl!,
                CreatedAt = DateTime.UtcNow,
                IsCompleted = false
            };

            await _jobRepo.SaveJobAsync(job);

            return result;
        }

        public async Task<JobResult> GetJobResultAsync(string jobId)
        {
            var baseUrl = _config["Adobe:GetJobResultAsyncEndpoint"];
            var clientId = _config["Adobe:ClientId"];
            var token = await _authService.GetAccessTokenAsync();

            var requestUrl = $"{baseUrl}{jobId}";

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("x-api-key", clientId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Firefly job result failed. Status: {StatusCode}, Body: {Body}", response.StatusCode, errorBody);
                return null;
            }

            var stream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<JobResult>(stream);

            return result;
        }

        public async Task<ExpandImageResponse> ExpandImageAsync(ExpandImageRequest request)
        {
            var baseUrl = _config["Adobe:ExpandAsyncEndpoint"];
            var clientId = _config["Adobe:ClientId"];
            var token = await _authService.GetAccessTokenAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var jsonBody = JsonSerializer.Serialize(request, options);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, baseUrl);
            httpRequest.Headers.Add("x-api-key", clientId);
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            Console.WriteLine(jsonBody);

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            
            var result = JsonSerializer.Deserialize<ExpandImageResponse>(json);
            
            if (result == null || string.IsNullOrWhiteSpace(result.JobId))
            {
                _logger.LogWarning("Received null or invalid job result from Firefly.");
                throw new Exception("Invalid response from Firefly.");
            }
            
            var job = new ImageGenerationJob
            {
                JobId = result.JobId!,
                Prompt = request.Prompt,
                StatusUrl = result.StatusUrl!,
                CancelUrl = result.CancelUrl!,
                CreatedAt = DateTime.UtcNow,
                IsCompleted = false
            };
            await _jobRepo.SaveJobAsync(job);

            return result;
        }

        public async Task<UploadImageToFireFlyResponse> UploadImageAsync(IFormFile file)
        {
            var baseURL = _config["Adobe:FireFlyBaseURL"];
            var clientId = _config["Adobe:ClientId"];
            var token = await _authService.GetAccessTokenAsync();

            using var content = new StreamContent(file.OpenReadStream());
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Headers.ContentLength = file.Length;

            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseURL}/v2/storage/image");
            request.Headers.Add("x-api-key", clientId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UploadImageToFireFlyResponse>(json)!;
        }

        public async Task<List<GenerateImageResponse>> GenerateBulkImagesAsync(List<GenerateImageRequest> requests)
        {
            var results = new List<GenerateImageResponse>();

            foreach (var request in requests)
            {
                try
                {
                    var result = await GenerateImageAsync(request);
                    results.Add(result);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to generate image for prompt: {Prompt}", request.Prompt);
                }
            }

            return results;
        }


    }
}