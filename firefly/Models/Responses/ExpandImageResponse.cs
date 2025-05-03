using System.Text.Json.Serialization;

namespace firefly.Models.Responses
{
    public class ExpandImageResponse
    {
        [JsonPropertyName("jobId")]
        public string JobId { get; set; }

        [JsonPropertyName("statusUrl")]
        public string StatusUrl { get; set; }

        [JsonPropertyName("cancelUrl")]
        public string CancelUrl { get; set; }
    }
}
