using System.Diagnostics;
using System.Reflection;
using BetterWin11_Builder;
using BetterWin11_Builder.Extraction;
using BetterWin11_Builder.Hives;
using BetterWin11_Builder.Stages;

Config.AcquirePrivileges();

Config.BaseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
Config.Img = Path.Combine(Config.BaseDir, "Img");
Config.Mnt = Path.Combine(Config.BaseDir, "Mnt");

var path = args[0];
var dir = Path.GetDirectoryName(path);

Console.WriteLine("[Extracting image]");
Directory.CreateDirectory(Config.Img);
IsoExtractor.ExtractTo(path, Config.Img);

var wim = Path.Combine(Config.Img, "sources", "install.wim");
var p = Process.Start(new ProcessStartInfo
{
    FileName = "dism",
    Arguments = $"/get-imageinfo /imagefile:\"{wim}\"",
    UseShellExecute = false,
    CreateNoWindow = false,
    RedirectStandardOutput = true
});

var lines = new List<string>();
while (p?.StandardOutput.ReadLine() is { } temp)
    lines.Add(temp);

var output = lines
    .Where(x => x.StartsWith("Name"))
    .Select(x => x.Split(':')[1].Trim())
    .ToList();

for (var i = 0; i < output.Count; i++)
{
    var edition = output[i];
    Console.WriteLine($"{i + 1}. {edition}");
}

Console.Write("Please select the edition you wish to modify: ");
Config.Index = int.Parse(Console.ReadLine());
Config.Edition = output[Config.Index - 1];

Console.WriteLine("[Mounting WIM file]");
Directory.CreateDirectory(Config.Mnt);
// TODO: /optimize?
Utils.StartSilent("dism", $"/mount-image /imagefile:\"{Path.Combine(Config.Img, "sources", "install.wim")}\" /index:{Config.Index} /mountdir:\"{Config.Mnt}\"");

Console.WriteLine("[Mounting registry hives]");
Config.Software = new Hive(Path.Combine(Config.Mnt, "Windows", "System32", "config", "SOFTWARE"));
Config.System = new Hive(Path.Combine(Config.Mnt, "Windows", "System32", "config", "SYSTEM"));

var stages = new Stage[]
{
    new DisableServices(),
    new DisableAutoPlay(),
    new DisablePrefetching(),
    new SetUpWinUpdate(),
    new DisableAutoRebootForBsod(),
    new DisableTelemetry(),
    new RemoveOneDrive(),
    new RemoveEdge(),
    new RemoveTeams(),
    new UnattendedSetup(),
    new RemoveLegacyBiosSupport(),
    new TaskbarTweaks(),
    new DontLetAppsRunInTheBackground()
};

foreach (var stage in stages)
{
    Console.WriteLine($"[{stage.Name}]");
    stage.Run();
}

Console.WriteLine("[Unmounting registry hives]");
Config.Software.SaveAndUnload();
Config.System.SaveAndUnload();

stages = new Stage[]
{
    new DisableWinDefender(),
    new RemoveLegacyFeatures(),
    new ReplaceApps()
};

foreach (var stage in stages)
{
    Console.WriteLine($"[{stage.Name}]");
    stage.Run();
}

/*Console.WriteLine("Cleaning up the image...");
Process.Start("dism", "/image:Mnt /cleanup-image /startcomponentcleanup /resetbase").WaitForExit();

Console.WriteLine("Optimizing AppX packages...");
Process.Start("dism", "/image:Mnt /optimize-provisionedappxpackages").WaitForExit();*/

Console.WriteLine("[Unmounting WIM image]");
Utils.StartSilent("dism", $"/unmount-image /mountdir:\"{Config.Mnt}\" /commit");

Directory.Delete(Config.Mnt);

if (Config.ExportAsEsd)
{
    var install = Path.Combine(Config.Img, "sources", "install.wim");
    Console.WriteLine("[Exporting WIM image as ESD]");
    Utils.StartSilent("dism", $"/export-image /sourceimagefile:\"{install}\" /sourceindex:{Config.Index} /destinationimagefile:\"{Path.Combine(Config.Img, "sources", "install.esd")}\" /destinationname:BetterWin11 /compress:recovery");
    File.Delete(install);
}
else
{
    var install = Path.Combine(Config.Img, "sources", "install.wim");
    var installNew = Path.Combine(Config.Img, "sources", "install.new.wim");
    Console.WriteLine("[Exporting WIM image]");
    Utils.StartSilent("dism", $"/export-image /sourceimagefile:\"{install}\" /sourceindex:{Config.Index} /destinationimagefile:\"{installNew}\" /destinationname:BetterWin11 /compress:maximum");
    File.Delete(install);
    File.Move(installNew, install);
}

/*if (Config.RemoveManualSetupSupport)
{
    foreach (var file in Directory.GetFiles("Img", "*.*", SearchOption.AllDirectories))
    {
        if (!file.Contains("Img\\efi")
            && !file.Contains("Img\\boot")
            && !file.EndsWith("boot.wim")
            && !file.EndsWith("install.wim")
            && !file.EndsWith("bootmgr")
            && !file.EndsWith("bootmgr.efi")
            )
            File.Delete(file);
    }
}*/

Console.WriteLine("[Creating bootable image]");
Utils.StartSilent(
    Path.Combine(Config.BaseDir, "oscdimg.exe"),
    $"-bootdata:{(Config.RemoveLegacyBiosSupport ? "1" : $"2#p0,e,b\"{Path.Combine(Config.Img, "boot", "etfsboot.com")}\"#")}pEF,e,b\"{Path.Combine(Config.Img, "efi", "microsoft", "boot", "Efisys.bin")}\" -h -m -o -u2 -udfver102 \"{Config.Img}\" \"{Path.Combine(dir, "BetterWin11.iso")}\" -lBetterWin11"
);

Directory.Delete(Config.Img, true);

Console.WriteLine("Finished! Press any key to exit...");
Console.ReadKey();

Config.ReturnPrivileges();