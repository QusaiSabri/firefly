using System.Text.Json;

public class AuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public AuthService(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var client = _httpClientFactory.CreateClient();

        var dict = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _config["Adobe:ClientId"]! },
            { "client_secret", _config["Adobe:ClientSecret"]! },
            //{ "scope", "openid AdobeID session additional_info.read organizations.read firefly_api ff_apis" }
            { "scope", "openid AdobeID firefly_enterprise firefly_api ff_apis" }
        };

        var content = new FormUrlEncodedContent(dict);

        var response = await client.PostAsync(_config["Adobe:TokenEndpoint"], content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty("access_token").GetString()!;
    }
}