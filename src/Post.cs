using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Renderers;
using Markdig.Syntax;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace anggape;

public sealed class Post
{
    public readonly string Path;
    public readonly string Title;
    public readonly string Content;
    public readonly string[] Tags;
    public readonly DateTime Added;
    public readonly DateTime Modified;

    private Post(
        string path,
        string title,
        string content,
        string[] tags,
        DateTime added,
        DateTime modified
    )
    {
        Path = path;
        Title = title;
        Content = content;
        Tags = tags;
        Added = added;
        Modified = modified;
    }

    public static Post Parse(string path)
    {
        using var writer = new StringWriter();
        var renderer = new HtmlRenderer(writer);
        var pipeline = new MarkdownPipelineBuilder().UseYamlFrontMatter().Build();
        pipeline.Setup(renderer);

        var document = Markdown.Parse(File.ReadAllText(path), pipeline);
        var yaml = document.Descendants<YamlFrontMatterBlock>().First().Lines.ToString();
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var meta = deserializer.Deserialize<Meta>(yaml);
        renderer.Render(document);

        var dates = exec("git", "log", "--pretty=format:%ad", "--date=iso8601", "--", path)
            .Split('\n');
        var added = string.IsNullOrEmpty(dates.Last())
            ? DateTime.Now
            : DateTime.Parse(dates.Last());
        var modified = string.IsNullOrEmpty(dates.First())
            ? DateTime.Now
            : DateTime.Parse(dates.First());

        return new Post(
            path: path,
            title: meta.Title,
            content: writer.ToString(),
            tags: meta.Tags,
            added: added,
            modified: modified
        );
    }
}
