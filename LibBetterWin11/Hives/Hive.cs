using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace BetterWin11_Builder.Hives;

public class Hive
{
    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern int RegLoadKey(IntPtr hKey, string lpSubKey, string lpFile);

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern int RegSaveKey(IntPtr hKey, string lpFile, uint securityAttrPtr = 0);

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern int RegUnLoadKey(IntPtr hKey, string lpSubKey);

    private readonly RegistryKey _parentKey;
    private readonly string _path, _name;

    public readonly RegistryKey? RootKey;

    public Hive(string path)
    {
        _parentKey = RegistryKey.OpenBaseKey(RegistryHive.Users, RegistryView.Default);
        _path = path;
        _name = "WIN11_" + Path.GetFileNameWithoutExtension(path);

        RegLoadKey(_parentKey.Handle.DangerousGetHandle(), _name, path);
        RootKey = _parentKey.OpenSubKey(_name, true);
    }

    public void SaveAndUnload()
    {
        RootKey?.Close();

        var handle = _parentKey.Handle.DangerousGetHandle();
        RegUnLoadKey(handle, _name);
        RegSaveKey(handle, _path);

        _parentKey.Close();
    }
}