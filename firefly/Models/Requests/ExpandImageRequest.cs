namespace firefly.Models.Requests
{
    public class ExpandImageRequest
    {
        public ExpandImage Image { get; set; }
        public ExpandMask? Mask { get; set; }
        public int NumVariations { get; set; } = 1;
        public Placement? Placement { get; set; }
        public string? Prompt { get; set; }
        public List<int>? Seeds { get; set; }
        public ExpandSize? Size { get; set; }
    }

    public class ExpandImage
    {
        public ImageSource? Mask { get; set; }
        public ImageSource Source { get; set; }
    }

    public class ExpandMask
    {
        public bool Invert { get; set; }
        public ImageSource Source { get; set; }
    }

    public class ImageSource
    {
        public string? UploadId { get; set; }
        public string? Url { get; set; }
    }

    public class Placement
    {
        public Alignment Alignment { get; set; }
        public Inset Inset { get; set; }
    }

    public class Alignment
    {
        public string Horizontal { get; set; } 
        public string Vertical { get; set; } 
    }

    public class Inset
    {
        public int Bottom { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
    }

    public class ExpandSize
    {
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
