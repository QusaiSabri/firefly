using System.Text.Json;

namespace firefly.Services
{
    public class FireflyStorageService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly AuthService _authService;

        public FireflyStorageService(IHttpClientFactory httpClientFactory, IConfiguration config, AuthService authService)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _authService = authService;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var token = await _authService.GetAccessTokenAsync();
            var client = _httpClientFactory.CreateClient();

            using var stream = file.OpenReadStream();
            using var content = new StreamContent(stream);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

            var request = new HttpRequestMessage(HttpMethod.Post, _config["Adobe:UploadEndpoint"])
            {
                Content = content
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("x-api-key", _config["Adobe:ClientId"]);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("images")[0].GetProperty("id").GetString()!;
        }
    }
}
