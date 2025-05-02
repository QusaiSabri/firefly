using firefly.Models.Enums;

namespace firefly.Models
{
    public class GenerateImageRequest
    {
        public string Prompt { get; set; } = string.Empty;
        public string ReferenceImageId { get; set; } = string.Empty;
        public TaskType TaskType { get; set; } = TaskType.Generate;
    }
}
