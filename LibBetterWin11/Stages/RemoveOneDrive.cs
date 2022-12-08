namespace BetterWin11_Builder.Stages;

public class RemoveOneDrive : Stage
{
    public override string Name => "Remove OneDrive";

    public override void Run()
    {
        File.Delete(Path.Combine(Config.Mnt, "Windows", "System32", "OneDriveSetup.exe"));
        File.Delete(Path.Combine(Config.Mnt, "Windows", "System32", "OneDrive.ico"));
    }
}