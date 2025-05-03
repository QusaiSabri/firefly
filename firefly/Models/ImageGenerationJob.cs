namespace firefly.Models
{
    public class ImageGenerationJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string JobId { get; set; }
        public string Prompt { get; set; } = default!;
        public string StatusUrl { get; set; } = default!;
        public string CancelUrl { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; } 

        public ICollection<ImageAsset> Assets { get; set; } = new List<ImageAsset>();
    }

}
