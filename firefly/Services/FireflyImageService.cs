//using System.Net.Http.Headers;
//using System.Text;
//using System.Text.Json;

//public class FireflyImageService
//{
//    private readonly IHttpClientFactory _httpClientFactory;
//    private readonly IConfiguration _config;
//    private readonly AuthService _authService;

//    public FireflyImageService(IHttpClientFactory httpClientFactory, IConfiguration config, AuthService authService)
//    {
//        _httpClientFactory = httpClientFactory;
//        _config = config;
//        _authService = authService;
//    }

//    public async Task<string> GenerateImageAsync(string prompt, string referenceImageId)
//    {
//        var token = await _authService.GetAccessTokenAsync();
//        var client = _httpClientFactory.CreateClient();

//        var body = new
//        {
//            contentClass = "photo",
//            numVariations = 1,
//            prompt,
//            seeds = new[] { 0 },
//            size = new
//            {
//                height = 1024,
//                width = 1024
//            },
//            //structure = new
//            //{
//            //    imageReference = new
//            //    {
//            //        source = new
//            //        {
//            //            //uploadId = referenceImageId,
//            //            url = referenceImageId
//            //        }
//            //    },
//            //    strength = 100
//            //},
//            style = new
//            {
//                imageReference = new
//                {
//                    source = new
//                    {
//                        //uploadId = referenceImageId,
//                        url = referenceImageId
//                    }
//                },
//                //presets = new string[] { },
//                strength = 1
//            },
//            //tileable = true,
//            visualIntensity = 2
//        };


//        var request = new HttpRequestMessage(HttpMethod.Post, _config["Adobe:GenerateEndpoint"]);
//        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
//        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//        request.Headers.Add("x-api-key", _config["Adobe:ClientId"]);
//        request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

//        var response = await client.SendAsync(request);

//        if (!response.IsSuccessStatusCode)
//        {
//            var error = await response.Content.ReadAsStringAsync();
//            Console.WriteLine($"Error: {error}");
//        }

//        response.EnsureSuccessStatusCode();

//        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
//        return json.GetProperty("outputs")[0].GetProperty("image").GetProperty("url").GetString()!;
//    }

//    public async Task<string> RemoveBackgroundAsync(string imageUrl)
//    {
//        var token = await _authService.GetAccessTokenAsync();
//        var client = _httpClientFactory.CreateClient();
//        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//        client.DefaultRequestHeaders.Add("x-api-key", _config["Adobe:ClientId"]);
//        var body = new
//        {
//            input = new { url = imageUrl },
//            output_format = "png"
//        };

//        Console.WriteLine($"Token: {token}");

//        string? requestUri = _config["Adobe:RemoveBackgroundEndpoint"];
//        var response = await client.PostAsJsonAsync(requestUri, body);
//        response.EnsureSuccessStatusCode();
//        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
//        return json.GetProperty("asset_url").GetString()!;
//    }
//}