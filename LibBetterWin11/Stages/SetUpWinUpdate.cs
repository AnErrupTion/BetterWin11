using Microsoft.Win32;

namespace BetterWin11_Builder.Stages;

public class SetUpWinUpdate : Stage
{
    public override string Name => "Set up Windows Update";

    public override void Run()
    {
        if (Config.DisableAutomaticUpdates)
        {
            using var key = Config.Software.RootKey?.CreateSubKey("Policies\\Microsoft\\Windows\\WindowsUpdate\\AU", true);

            key?.SetValue("AUOptions", 2, RegistryValueKind.DWord);
            key?.SetValue("NoAutoUpdate", 1, RegistryValueKind.DWord);
        }
        if (Config.DisableDriverUpdatesFromWinUpdate)
        {
            using var key = Config.Software.RootKey?.CreateSubKey("Policies\\Microsoft\\Windows\\WindowsUpdate", true);
            key?.SetValue("ExcludeWUDriversInQualityUpdate", 1, RegistryValueKind.DWord);
        }
        if (Config.DisableAutoRebootForUpdates)
        {
            using var key = Config.Software.RootKey?.CreateSubKey("Policies\\Microsoft\\Windows\\WindowsUpdate\\AU", true);
            key?.SetValue("NoAutoRebootWithLoggedOnUsers", 1, RegistryValueKind.DWord);
        }
        if (Config.DisableUpdatesForOtherMicrosoftProducts)
        {
            // TODO: Is this correct?
            using var key = Config.Software.RootKey?.CreateSubKey("Policies\\Microsoft\\Windows\\WindowsUpdate\\AU", true);
            key?.SetValue("AllowMUUpdateService", 0, RegistryValueKind.DWord);
        }
        if (Config.DisableMrt)
        {
            using var key = Config.Software.RootKey?.CreateSubKey("Policies\\Microsoft\\MRT", true);
            key?.SetValue("DontOfferThroughWUAU", 1, RegistryValueKind.DWord);
        }
    }
}