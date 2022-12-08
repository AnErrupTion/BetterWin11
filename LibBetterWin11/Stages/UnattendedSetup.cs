namespace BetterWin11_Builder.Stages;

public class UnattendedSetup : Stage
{
    public override string Name => "Unattended Setup";

    public override void Run()
    {
        var answerFile = File.ReadAllLines(Path.Combine(Config.BaseDir, "autounattend.xml")).ToList();
        
        if (!(Config.Edition.Contains("Enterprise") || Config.Edition.Contains("Evaluation")))
        {
            var userData = answerFile.IndexOf("			<UserData>") + 1;
            answerFile.Insert(userData, "				<ProductKey>");
            answerFile.Insert(userData + 1, "					<Key></Key>");
            answerFile.Insert(userData + 2, "					<WillShowUI>Never</WillShowUI>");
            answerFile.Insert(userData + 3, "				</ProductKey>");
        }

        var oobe = answerFile.IndexOf("			<OOBE>") + 1;
        if (Config.UseLocalAccount) answerFile.Insert(oobe, "				<HideOnlineAccountScreens>true</HideOnlineAccountScreens>");
        if (Config.BypassPrivacyOptions) answerFile.Insert(oobe, "				<ProtectYourPC>3</ProtectYourPC>");

        File.WriteAllLines(Path.Combine(Config.Img, "autounattend.xml"), answerFile);
    }
}