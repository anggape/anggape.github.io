namespace anggape;

public sealed class Meta
{
    public string Title { get; set; } = string.Empty;
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string? Slug { get; set; }
}
