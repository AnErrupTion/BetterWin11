using System.Diagnostics;

namespace BetterWin11_Builder.Stages;

public class ReplaceApps : Stage
{
    public override string Name => "Replace applications";

    public override void Run()
    {
        var p = Process.Start(new ProcessStartInfo
        {
            FileName = "dism",
            Arguments = $"/image:\"{Config.Mnt}\" /get-provisionedappxpackages",
            UseShellExecute = false,
            CreateNoWindow = false,
            RedirectStandardOutput = true
        });

        var lines = new List<string>();
        while (p?.StandardOutput.ReadLine() is { } temp)
            lines.Add(temp);

        var output = lines
            .Where(x => x.StartsWith("PackageName"))
            .Select(x => x.Split(':')[1].Trim());

        // Remove requested apps
        foreach (var line in output)
            if ((Config.RemoveClipChamp && line.Contains("Clipchamp"))
                || (Config.RemoveTeams && line.Contains("MicrosoftTeams"))
                || (Config.RemoveEdge && line.Contains("MicrosoftEdge"))
                || (Config.RemoveOfficeHub && line.Contains("Office"))
               )
                Utils.StartSilent("dism", $"/image:\"{Config.Mnt}\" /remove-provisionedappxpackage /packagename:" + line);

        // Replace other apps with their Store equivalent
        if (!Config.RemoveOneDrive) Utils.StartSilent("dism", $"/image:\"{Config.Mnt}\" /add-provisionedappxpackage /packagepath:\"{Path.Combine(Config.BaseDir, "OneDrive.appxbundle")}\" /skiplicense");
        if (Config.InstallFirefox) Utils.StartSilent("dism", $"/image:\"{Config.Mnt}\" /add-provisionedappxpackage /packagepath:\"{Path.Combine(Config.BaseDir, "Firefox.msix")}\" /skiplicense");
    }
}