namespace DFBackup.Runner.Models;

public class ValidateSettingsResult
{
    public List<string> ValidationResults { get; set; } = new List<string>();
    public bool ValidFile { get; set; }
}
