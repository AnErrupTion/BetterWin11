namespace BetterWin11_Builder.Stages;

public class DisableWinDefender : Stage
{
    public override string Name => "Disable Windows Defender";

    public override void Run()
    {
        if (Config.DisableWinDefender)
        {
            Utils.StartSilent("dism", $"/image:\"{Config.Mnt}\" /disable-feature:Windows-Defender-Default-Definitions");
            Utils.StartSilent("dism", $"/image:\"{Config.Mnt}\" /disable-feature:Windows-Defender-ApplicationGuard");
        }
    }
}