
using Microsoft.Win32;

namespace BetterWin11_Builder.Stages;

public class RemoveTeams : Stage
{
    public override string Name => "Remove Teams";

    public override void Run()
    {
        if (Config.RemoveTeams)
        {
            // Thank you https://www.reddit.com/r/sysadmin/comments/q771i4/comment/hnslzp5/?utm_source=share&utm_medium=web2x&context=3!!!!
            // This disables automatic Microsoft Teams installation after installation
            // NOTE: Accessing this registry key requires special privileges
            using var key = Config.Software.RootKey?.CreateSubKey("Microsoft\\Windows\\CurrentVersion\\Communications", true);
            key?.SetValue("ConfigureChatAutoInstall", 0, RegistryValueKind.DWord);

            // This disables the chat icon
            using var key2 = Config.Software.RootKey?.CreateSubKey("Policies\\Microsoft\\Windows\\Windows Chat", true);
            key2?.SetValue("ChatIcon", 3, RegistryValueKind.DWord);
        }
    }
}