namespace BetterWin11_Builder.Stages;

public class RemoveLegacyBiosSupport : Stage
{
    public override string Name => "Remove legacy BIOS support";

    public override void Run()
    {
        if (Config.RemoveLegacyBiosSupport)
        {
            //Directory.Delete("Img/boot", true);
            File.Delete(Path.Combine(Config.Img, "bootmgr"));
            Directory.Delete(Path.Combine(Config.Mnt, "Windows", "Boot", "PCAT"), true);
            Directory.Delete(Path.Combine(Config.Mnt, "Windows", "Boot", "DVD", "PCAT"), true);
            Directory.Delete(Path.Combine(Config.Mnt, "Windows", "Boot", "Misc"), true);
        }
    }
}