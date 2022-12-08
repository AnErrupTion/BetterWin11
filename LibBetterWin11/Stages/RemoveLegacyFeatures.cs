using System.Diagnostics;

namespace BetterWin11_Builder.Stages;

public class RemoveLegacyFeatures : Stage
{
    public override string Name => "Remove legacy features";

    public override void Run()
    {
        if (Config.RemoveLegacyMediaPlayer) Utils.StartSilent("dism", $"/image:\"{Config.Mnt}\" /disable-feature:WindowsMediaPlayer /remove");
        if (Config.RemoveLegacyComponents) Utils.StartSilent("dism", $"/image:\"{Config.Mnt}\" /disable-feature:LegacyComponents /remove");
        if (Config.RemoveDirectPlay) Utils.StartSilent("dism", $"/image:\"{Config.Mnt}\" /disable-feature:DirectPlay /remove");

        var p = Process.Start(new ProcessStartInfo
        {
            FileName = "dism",
            Arguments = $"/image:\"{Config.Mnt}\" /get-packages",
            UseShellExecute = false,
            CreateNoWindow = false,
            RedirectStandardOutput = true
        });

        var lines = new List<string>();
        while (p?.StandardOutput.ReadLine() is { } temp)
            lines.Add(temp);

        var output = lines
            .Where(x => x.StartsWith("Package Identity"))
            .Select(x => x.Split(':')[1].Trim());

        foreach (var line in output)
            if ((Config.RemoveInternetExplorer && line.Contains("InternetExplorer"))
                || (Config.RemoveLegacyNotepad && line.Contains("Notepad"))
                || (Config.RemoveLegacyMediaPlayer && line.Contains("MediaPlayer"))
                || (Config.RemoveWordPad && line.Contains("WordPad"))
               )
                Utils.StartSilent("dism", $"/image:\"{Config.Mnt}\" /remove-package /packagename:" + line);
    }
}