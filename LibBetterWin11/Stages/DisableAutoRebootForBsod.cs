using Microsoft.Win32;

namespace BetterWin11_Builder.Stages;

public class DisableAutoRebootForBsod : Stage
{
    public override string Name => "Disable automatic reboots for BSODs";

    public override void Run()
    {
        if (Config.DisableAutoRebootForBsod)
        {
            using var key = Config.System.RootKey?.CreateSubKey("ControlSet001\\Control\\CrashControl", true);
            key?.SetValue("AutoReboot", 0, RegistryValueKind.DWord);
        }
    }
}