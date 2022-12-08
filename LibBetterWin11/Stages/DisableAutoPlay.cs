using Microsoft.Win32;

namespace BetterWin11_Builder.Stages;

public class DisableAutoPlay : Stage
{
    public override string Name => "Disable AutoPlay";

    public override void Run()
    {
        if (Config.DisableAutoPlay)
        {
            using var key = Config.Software.RootKey?.CreateSubKey("Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
            key?.SetValue("NoDriveTypeAutoRun", 255, RegistryValueKind.DWord);
        }
    }
}