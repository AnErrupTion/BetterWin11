using Microsoft.Win32;

namespace BetterWin11_Builder.Stages;

public class DontLetAppsRunInTheBackground : Stage
{
    public override string Name => "Don't let apps run in the background";

    public override void Run()
    {
        if (Config.DontLetAppsRunInTheBackground)
        {
            using var key = Config.Software.RootKey?.CreateSubKey("Microsoft\\Windows\\CurrentVersion\\BackgroundAccessApplications", true);
            key?.SetValue("GlobalUserDisabled", 1, RegistryValueKind.DWord);

            using var key2 = Config.Software.RootKey?.CreateSubKey("Microsoft\\Windows\\CurrentVersion\\Search", true);
            key2?.SetValue("BackgroundAppGlobalToggle", 0, RegistryValueKind.DWord);
        }
    }
}