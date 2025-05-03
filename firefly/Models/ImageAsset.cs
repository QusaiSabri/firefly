namespace firefly.Models
{
    public class ImageAsset
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string Prompt { get; set; }
        public ImageGenerationJob Job { get; set; } = default!;
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Seed { get; set; }
        public string? BlobPath { get; set; }
        public string? ContentClass { get; set; }
    }
}
