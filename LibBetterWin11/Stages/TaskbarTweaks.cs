using Microsoft.Win32;

namespace BetterWin11_Builder.Stages;

public class TaskbarTweaks : Stage
{
    public override string Name => "Taskbar Tweaks";

    public override void Run()
    {
        if (Config.AlignTaskbarToLeft)
        {
            using var key = Config.Software.RootKey?.CreateSubKey("Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", true);
            key?.SetValue("TaskbarAl", 0, RegistryValueKind.DWord);
        }
        if (Config.HideSearchButton)
        {
            using var key = Config.Software.RootKey?.CreateSubKey("Microsoft\\Windows\\CurrentVersion\\Search", true);
            key?.SetValue("SearchboxTaskbarMode", 0, RegistryValueKind.DWord);
        }
        if (Config.HideTaskViewButton)
        {
            using var key = Config.Software.RootKey?.CreateSubKey("Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", true);
            key?.SetValue("ShowTaskViewButton", 0, RegistryValueKind.DWord);
        }
        if (Config.HideWidgetsButton)
        {
            using var key = Config.Software.RootKey?.CreateSubKey("Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", true);
            key?.SetValue("TaskbarDa", 0, RegistryValueKind.DWord);
        }
    }
}