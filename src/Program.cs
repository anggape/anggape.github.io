using anggape;
using Scriban;

var outputPath = "dist";
var postOutputPath = Path.Combine(outputPath, "post");
var tagOutputPath = Path.Combine(outputPath, "tag");
var layoutPath = "src/template/layout.html";
var indexPath = "src/template/index.html";
var postPath = "src/template/post.html";
var layoutTemplate = Template.Parse(File.ReadAllText(layoutPath), layoutPath);
var indexTemplate = Template.Parse(File.ReadAllText(indexPath), indexPath);
var postTemplate = Template.Parse(File.ReadAllText(postPath), postPath);

var posts = Directory
    .EnumerateFiles("posts", "*.md", SearchOption.AllDirectories)
    .Where(x => !x.EndsWith(".draft.md"))
    .Select(Path.GetFullPath)
    .Select(Post.Parse)
    .OrderBy(x => x.Added)
    .ToArray();

if (!Directory.Exists(outputPath))
    Directory.CreateDirectory(outputPath);
if (!Directory.Exists(postOutputPath))
    Directory.CreateDirectory(postOutputPath);
if (!Directory.Exists(tagOutputPath))
    Directory.CreateDirectory(tagOutputPath);

File.WriteAllText(
    Path.Combine(outputPath, "index.html"),
    layoutTemplate.Render(
        new
        {
            Page = new { Title = "anggape" },
            Tags = Tag.All,
            Content = indexTemplate.Render(new { Posts = posts })
        }
    )
);

foreach (var post in posts)
{
    File.WriteAllText(
        Path.Combine(postOutputPath, $"{post.Slug}.html"),
        layoutTemplate.Render(
            new
            {
                Page = new { Title = post.Title },
                Tags = Tag.All,
                Content = postTemplate.Render(new { Post = post })
            }
        )
    );
}

foreach (var tag in Tag.All)
{
    Console.WriteLine(tag.Name + " " + tag.Posts.Count);
    File.WriteAllText(
        Path.Combine(tagOutputPath, $"{tag.Slug}.html"),
        layoutTemplate.Render(
            new
            {
                Page = new { Title = tag.Name },
                Tags = Tag.All,
                Content = indexTemplate.Render(new { Posts = tag.Posts })
            }
        )
    );
}
