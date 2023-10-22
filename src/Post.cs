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

    private Post(string path, string title, string content, string[] tags)
    {
        Path = path;
        Title = title;
        Content = content;
        Tags = tags;
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

        return new Post(path: path, title: meta.Title, content: writer.ToString(), tags: meta.Tags);
    }
}
