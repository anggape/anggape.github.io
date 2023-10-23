global using static anggape.Helper;

using System.Diagnostics;
using System.Text.RegularExpressions;

namespace anggape;

public static class Helper
{
    public static string exec(string file, params string[] args)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = file,
                Arguments = string.Join(' ', args),
                WorkingDirectory = Directory.GetCurrentDirectory(),
                RedirectStandardOutput = true,
            },
            EnableRaisingEvents = true,
        };
        process.Start();
        process.WaitForExit();

        return process.StandardOutput.ReadToEnd();
    }

    public static string slug(string value)
    {
        value = Regex.Replace(value.ToLower(), @"[^a-z0-9\s-]", string.Empty);
        value = Regex.Replace(value, @"\s+", " ");
        return Regex.Replace(value, @"\s", "-");
    }
}
