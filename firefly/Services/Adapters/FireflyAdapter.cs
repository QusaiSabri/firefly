using firefly.Models;
using firefly.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace firefly.Services.Adapters
{
    public class FireflyAdapter : IImageGenerationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly AuthService _authService;
        private readonly ILogger<FireflyAdapter> _logger;


        public FireflyAdapter(HttpClient httpClient, IConfiguration config, AuthService authService)
        {
            _httpClient = httpClient;
            _config = config;
            _authService = authService;
        }
        public async Task<GenerateImageResponse> GenerateImageAsync(GenerateImageRequest request)
        {
            var baseUrl = _config["Adobe:GenerateAsyncEndpoint"];
            var clientId = _config["Adobe:ClientId"];
            var clientSecret = _config["Firefly:ClientSecret"];
            var token = await _authService.GetAccessTokenAsync();

            var body = new
            {
                prompt = request.Prompt,
                contentClass = "photo",
                numVariations = 1,
                //style = request.ReferenceImageId != null ? new
                //{
                //    imageReference = new
                //    {
                //        source = new
                //        {
                //            uploadId = request.ReferenceImageId
                //        }
                //    }
                //} : null
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, baseUrl);
            httpRequest.Headers.Add("x-api-key", clientId);
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<GenerateImageResponse>(json);
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
    }
}