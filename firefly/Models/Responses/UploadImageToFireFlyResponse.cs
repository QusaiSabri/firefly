namespace firefly.Models.Responses
{
    public class UploadImageToFireFlyResponse
    {
        public List<UploadedImage> Images { get; set; } = new();
    }

    public class UploadedImage
    {
        public string Id { get; set; } = default!;
    }
}
