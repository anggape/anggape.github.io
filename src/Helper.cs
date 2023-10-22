global using static anggape.Helper;

using System.Diagnostics;

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
}
