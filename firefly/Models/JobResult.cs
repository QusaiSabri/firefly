using System.Text.Json.Serialization;

namespace firefly.Models
{
    public class JobResult
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("jobId")]
        public string JobId { get; set; }

        [JsonPropertyName("result")]
        public FireflyResult Result { get; set; }
    }

    public class FireflyResult
    {
        [JsonPropertyName("size")]
        public FireflyImageSize Size { get; set; }

        [JsonPropertyName("outputs")]
        public List<FireflyImageOutput> Outputs { get; set; }

        [JsonPropertyName("contentClass")]
        public string ContentClass { get; set; }
    }

    public class FireflyImageSize
    {
        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }
    }

    public class FireflyImageOutput
    {
        [JsonPropertyName("seed")]
        public int Seed { get; set; }

        [JsonPropertyName("image")]
        public FireflyImageUrl Image { get; set; }
    }

    public class FireflyImageUrl
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

}
