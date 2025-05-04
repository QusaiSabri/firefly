using System.Text.Json.Serialization;

namespace firefly.Models.Responses
{
    public class UploadImageToFireFlyResponse
    {
        [JsonPropertyName("images")]
        public List<UploadedImage> Images { get; set; } = new();
    }

    public class UploadedImage
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;
    }
}
