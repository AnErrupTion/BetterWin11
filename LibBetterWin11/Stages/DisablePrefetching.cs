using Microsoft.Win32;

namespace BetterWin11_Builder.Stages;

public class DisablePrefetching : Stage
{
    public override string Name => "Disable Prefetching";

    public override void Run()
    {
        if (Config.DisablePrefetching)
        {
            using var key = Config.System.RootKey?.CreateSubKey("ControlSet001\\Control\\Session Manager\\Memory Management\\PrefetchParameters", true);

            key?.SetValue("EnablePrefetcher", 0, RegistryValueKind.DWord);
            key?.SetValue("EnableSuperfetcher", 0, RegistryValueKind.DWord);
        }
    }
}