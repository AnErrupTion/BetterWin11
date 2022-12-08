
namespace BetterWin11_Builder.Stages;

public class RemoveEdge : Stage
{
    public override string Name => "Remove Edge";

    public override void Run()
    {
        if (Config.RemoveEdge)
            Directory.Delete(Path.Combine(Config.Mnt, "Program Files (x86)", "Microsoft"), true);
    }
}