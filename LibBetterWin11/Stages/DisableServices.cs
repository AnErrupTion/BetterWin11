using Microsoft.Win32;

namespace BetterWin11_Builder.Stages;

public class DisableServices : Stage
{
    public override string Name => "Disable Services";

    public override void Run()
    {
        if (Config.DisableWinDefender)
        {
            Disable("SgrmBroker");
            Disable("WinDefend");
            Disable("WdNisSvc");
            Disable("WdNisDrv");
            Disable("WdBoot");
            Disable("WdFilter");
            Disable("webthreatdefsvc");
            Disable("webthreatdefusersvc_61adb");
            Disable("Sense");
        }
        if (Config.DisableWinUpdate)
        {
            Disable("wuauserv");
            Disable("UsoSvc");
            Disable("BITS");
            Disable("DoSvc");
            Disable("WaaSMedicSvc");
            Disable("TrustedInstaller");
        }
        if (Config.DisableMicrosoftAccounts)
        {
            Disable("wlidsvc");
            Disable("TokenBroker");
        }
        if (Config.DisableTelemetry)
        {
            Disable("diagnosticshub.standardcollector.service");
            Disable("DiagTrack");
        }
        if (Config.EnhancePrivacyAndSecurity)
        {
            Disable("lfsvc");
            Disable("fhsvc");
            Disable("OneSyncSvc_61adb");
            Disable("cloudidsvc");
            Disable("RasAuto");
            Disable("RasMan");
            Disable("SessionEnv");
            Disable("TermService");
            Disable("UmRdpService");
            Disable("RemoteAccess");
            Disable("UserDataSvc_61adb");
            Disable("InventorySvc");
        }
        if (Config.DisableDiagnostics)
        {
            Disable("diagsvc");

            // NOTE: Accessing those registry keys requires special privileges
            Disable("DPS");
            Disable("WdiServiceHost");
            Disable("WdiSystemHost");
            Disable("WdiSystemHost");

            Disable("wercplsupport");
            Disable("TroubleshootingSvc");
        }
        if (Config.DisableMicrosoftStore)
        {
            Disable("PushToInstall");
            Disable("InstallService");
            Disable("AppXSvc");
            Disable("AppReadiness");
        }
        if (Config.DisableSearchIndexing)
        {
            Disable("WSearch");
            Disable("PimIndexMaintenanceSvc_61adb");
        }
        if (Config.RemoveEdge)
        {
            Delete("edgeupdate");
            Delete("edgeupdatem");
        }
        if (Config.DisableSecurityHealth) Disable("SecurityHealthService");
        if (Config.DisableSecurityCenter) Disable("wscsvc");
        if (Config.DisableWinFirewall) Disable("mpssvc");
        if (Config.DisableErrorReporting) Disable("WerSvc");
        if (Config.DisableAutoPlay) Disable("ShellHWDetection");
        if (Config.DisablePrefetching) Disable("SysMain");
        if (Config.DisableNetBios) Disable("lmhosts");
        if (Config.DisableProgramCompatibilityAssistant) Disable("PcaSvc");
    }

    private static void Disable(string name)
    {
        using var key = Config.System.RootKey?.CreateSubKey("ControlSet001\\Services\\" + name, true);
        key?.SetValue("Start", 4, RegistryValueKind.DWord);
    }

    private static void Delete(string name)
    {
        Config.System.RootKey?.DeleteSubKeyTree("ControlSet001\\Services\\" + name, false);
    }
}