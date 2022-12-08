using System.Runtime.InteropServices;

namespace RegistryHiveRebuilder;

public static class Config
{
    public static void AcquirePrivileges()
    {
        SetPrivilege("SeRestorePrivilege", true);
        SetPrivilege("SeBackupPrivilege", true);
    }

    public static void ReturnPrivileges()
    {
        SetPrivilege("SeRestorePrivilege", false);
        SetPrivilege("SeBackupPrivilege", false);
    }

    [DllImport("ntdll.dll", SetLastError = true)]
    private static extern IntPtr RtlAdjustPrivilege(int privilege, bool bEnablePrivilege, bool isThreadPrivilege, out bool previousValue);

    [DllImport("advapi32.dll")]
    private static extern bool LookupPrivilegeValue(IntPtr lpSystemName, string lpName, ref ulong lpLuid);

    private static void SetPrivilege(string name, bool enabled)
    {
        ulong privilege = 0;
        LookupPrivilegeValue(IntPtr.Zero, name, ref privilege);
        RtlAdjustPrivilege((int)privilege, enabled, false, out _);
    }
}