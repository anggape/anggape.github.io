using anggape;

var posts = Directory
    .EnumerateFiles("posts", "*.md", SearchOption.AllDirectories)
    .Where(x => !x.EndsWith(".draft.md"))
    .Select(Path.GetFullPath)
    .Select(Post.Parse);

foreach (var post in posts)
{
    Console.WriteLine(
        post.Path
            + "\n"
            + "\t"
            + string.Join(" ,", post.Tags)
            + "\n"
            + "--------------------------------------"
            + "\n"
            + post.Content
            + "======================================"
            + "\n"
    );
}
