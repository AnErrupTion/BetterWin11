using System.Diagnostics;
using Microsoft.Win32;
using RegistryHiveRebuilder;

Config.AcquirePrivileges();

try
{
    RebuildHive("SOFTWARE");
    RebuildHive("SYSTEM");
    RebuildHive("SECURITY");
    RebuildHive("SAM");
    RebuildHive("ELAM");
    RebuildHive("DRIVERS");
    RebuildHive("DEFAULT");
    RebuildHive("COMPONENTS");
    RebuildHive("BBI");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

Config.ReturnPrivileges();

void RebuildHive(string path)
{
    var hive = Path.GetFileNameWithoutExtension(path);
    Console.WriteLine("Rebuilding hive: " + hive);

    var watch = new Stopwatch();
    watch.Start();

    var newPath = "OPT_" + hive;
    Process.Start(new ProcessStartInfo
    {
        FileName = "bcdedit",
        Arguments = "/createstore " + newPath,
        UseShellExecute = false,
        CreateNoWindow = false,
        RedirectStandardOutput = true
    })?.WaitForExit();

    var newHive = new Hive(newPath);
    newHive.RootKey?.DeleteSubKey("Description");
    newHive.RootKey?.DeleteSubKey("Objects");

    var oldHive = new Hive(path);

    OutputRegKey(oldHive.RootKey, newHive.RootKey);

    oldHive.SaveAndUnload();
    newHive.SaveAndUnload();

    watch.Stop();
    Console.WriteLine($"Took: {watch.Elapsed.Seconds} s.");
}

void ProcessValueNames(RegistryKey? key, RegistryKey? newKey)
{
    var names = key?.GetValueNames();
    if (names is not { Length: > 0 })
        return;

    foreach (var name in names)
    {
        var obj = key?.GetValue(name);
        if (obj != null)
        {
            using var subKey = newKey?.CreateSubKey(key?.Name, true);
            subKey?.SetValue(name, obj, key.GetValueKind(name));
        }
    }
}

void OutputRegKey(RegistryKey? key, RegistryKey? newKey)
{
    var names = key?.GetSubKeyNames();
    if (names is not { Length: > 0 })
    {
        ProcessValueNames(key, newKey);
        return;
    }

    foreach (var name in names)
    {
        try
        {
            using var key2 = key?.OpenSubKey(name);
            OutputRegKey(key2, newKey);
        }
        catch
        {
            Console.WriteLine($"Can't access sub key: {key?.Name}\\{name}");
        }
    }

    ProcessValueNames(key, newKey); 
}