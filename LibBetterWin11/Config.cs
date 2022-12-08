using System.Runtime.InteropServices;
using BetterWin11_Builder.Hives;

namespace BetterWin11_Builder;

public static class Config
{
    // Settings
    public const bool DisableWinDefender = true;
    public const bool DisableWinUpdate = false;
    public const bool DisableMicrosoftAccounts = false;
    public const bool DisableDiagnostics = true;
    public const bool DisableMicrosoftStore = false;
    public const bool DisableSearchIndexing = true;
    public const bool DisableSecurityHealth = false;
    public const bool DisableSecurityCenter = false;
    public const bool DisableWinFirewall = false;
    public const bool DisableErrorReporting = true;
    public const bool DisableAutoPlay = true;
    public const bool DisablePrefetching = true;
    public const bool DisableNetBios = true;
    public const bool DisableProgramCompatibilityAssistant = true;

    public const bool DisableAutomaticUpdates = false;
    public const bool DisableDriverUpdatesFromWinUpdate = true;
    public const bool DisableAutoRebootForUpdates = true;
    public const bool DisableUpdatesForOtherMicrosoftProducts = true;
    public const bool DisableMrt = true;

    public const bool DisableAutoRebootForBsod = true;

    public const bool DisableTelemetry = true;

    public const bool EnhancePrivacyAndSecurity = true;

    public const bool RemoveEdge = true; // TODO: Properly remove Edge
    public const bool RemoveOneDrive = true; // TODO: Properly remove OneDrive
    public const bool RemoveTeams = true;
    public const bool RemoveClipChamp = true;
    public const bool RemoveOfficeHub = true;
    public const bool DontLetAppsRunInTheBackground = true;

    public const bool InstallFirefox = true;

    public const bool RemoveInternetExplorer = true;
    public const bool RemoveLegacyNotepad = true;
    public const bool RemoveLegacyMediaPlayer = true;
    public const bool RemoveLegacyComponents = true;
    public const bool RemoveDirectPlay = true;
    public const bool RemoveWordPad = true;

    public const bool AlignTaskbarToLeft = true;
    public const bool HideSearchButton = true;
    public const bool HideTaskViewButton = true;
    public const bool HideWidgetsButton = true;

    public const bool UseLocalAccount = true;
    public const bool BypassPrivacyOptions = true;

    public const bool UseNewInstaller = true;
    public const bool AddBtrfsSupport = true;

    public const bool RebuildRegistryHives = true;

    public const bool RemoveLegacyBiosSupport = true;

    public const bool RemoveManualSetupSupport = true;

    public const bool ExportAsEsd = false;

    // Directories
    public static string BaseDir, Img, Mnt;

    // WIM info
    public static string Edition;
    public static int Index;

    // Registry hives
    public static Hive Software, System;

    // Privileges
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