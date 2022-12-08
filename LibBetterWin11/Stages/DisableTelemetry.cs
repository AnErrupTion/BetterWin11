using Microsoft.Win32;

namespace BetterWin11_Builder.Stages;

public class DisableTelemetry : Stage
{
    public override string Name => "Disable telemetry";

    public override void Run()
    {
        if (Config.DisableTelemetry)
        {
            using var key = Config.Software.RootKey?.CreateSubKey("Policies\\Microsoft\\Windows\\DataCollection", true);
            key?.SetValue("AllowTelemetry", 0, RegistryValueKind.DWord);
        }
    }
}