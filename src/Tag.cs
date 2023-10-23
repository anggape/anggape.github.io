namespace anggape;

public sealed class Tag
{
    public readonly string Name;
    public readonly List<Post> Posts;
    public string Slug => slug(Name);

    private static readonly Dictionary<string, Tag> s_AllTags = new Dictionary<string, Tag>();
    public static Tag[] All => s_AllTags.Values.ToArray();

    public Tag(string name)
    {
        Name = name;
        Posts = new List<Post>();
    }

    public static Tag Get(string name)
    {
        if (s_AllTags.ContainsKey(name))
            return s_AllTags[name];
        var tag = new Tag(name);
        s_AllTags.Add(name, tag);
        return tag;
    }
}
