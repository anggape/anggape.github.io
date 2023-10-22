using anggape;

var posts = Directory
    .EnumerateFiles("posts", "*.md", SearchOption.AllDirectories)
    .Where(x => !x.EndsWith(".draft.md"))
    .Select(Path.GetFullPath)
    .Select(Post.Parse);

foreach (var post in posts)
{
    Console.WriteLine(post.Path);
    Console.WriteLine("\tadded      : " + post.Added);
    Console.WriteLine("\tmodified   : " + post.Modified);
}

foreach (var tag in Tag.All)
{
    Console.WriteLine(tag.Name);
    Console.WriteLine("\t" + tag.Posts.Count);
}
