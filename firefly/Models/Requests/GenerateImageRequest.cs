using System.Text.Json.Serialization;

namespace firefly.Models.Requests
{
    public class GenerateImageRequest
    {
        [JsonPropertyName("prompt")]
        public required string Prompt { get; set; }

        [JsonPropertyName("contentClass")]
        public string? ContentClass { get; set; }

        [JsonPropertyName("customModelId")]
        public string? CustomModelId { get; set; }

        [JsonPropertyName("negativePrompt")]
        public string? NegativePrompt { get; set; }

        [JsonPropertyName("numVariations")]
        public int? NumVariations { get; set; }

        [JsonPropertyName("promptBiasingLocaleCode")]
        public string? PromptBiasingLocaleCode { get; set; }

        [JsonPropertyName("seeds")]
        public List<int>? Seeds { get; set; }

        [JsonPropertyName("size")]
        public FireflyImageSize? Size { get; set; }

        [JsonPropertyName("structure")]
        public StructureSettings? Structure { get; set; }

        [JsonPropertyName("style")]
        public StyleSettings? Style { get; set; }

        [JsonPropertyName("visualIntensity")]
        public int? VisualIntensity { get; set; }
    }

    public class StructureSettings
    {
        [JsonPropertyName("imageReference")]
        public ImageReference? ImageReference { get; set; }

        [JsonPropertyName("strength")]
        public int? Strength { get; set; }
    }

    public class StyleSettings
    {
        [JsonPropertyName("imageReference")]
        public ImageReference? ImageReference { get; set; }

        [JsonPropertyName("presets")]
        public List<string>? Presets { get; set; }

        [JsonPropertyName("strength")]
        public int? Strength { get; set; }
    }

    public class ImageReference
    {
        [JsonPropertyName("source")]
        public UploadSource? Source { get; set; }
    }

    public class UploadSource
    {
        [JsonPropertyName("uploadId")]
        public string? UploadId { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

    public class FireflyImageSize
    {
        [JsonPropertyName("width")]
        public int? Width { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }
    }
}
