using System.Diagnostics;

namespace BetterWin11_Builder;

public static class Utils
{
    public static void StartSilent(string name, string args)
    {
        var p = Process.Start(new ProcessStartInfo
        {
            FileName = name,
            Arguments = args,
            UseShellExecute = false,
            CreateNoWindow = false,
            RedirectStandardOutput = true
        });
        p?.StandardOutput.ReadToEnd();
    }
}