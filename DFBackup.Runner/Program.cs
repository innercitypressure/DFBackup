using CommandLine;
using DFBackup.Runner.Application;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed<Options>(o =>
    {
        ColorConsole.WriteWrappedHeader("Dwarf Fortress Backup Manager");
        
        if (o.About)
        {
            ColorConsole.WriteInfo($"Dwarf Fortress Backup Version {GetAssemblyVersion.Display()}");
            ColorConsole.WriteInfo("- Repo: https://github.com/innercitypressure/DFBackup");
        }

        if (o.Validate)
        {
            ColorConsole.WriteInfo("Validating settings.json....");

            var validationResults = ValidateSettings.Check();

            foreach (var result in validationResults.ValidationResults)
            {
                Console.WriteLine(result);
            }

            if (!validationResults.ValidFile)
            {
                ColorConsole.WriteError($"{Environment.NewLine}Settings.json file is NOT valid.");
            }
            else
            {
                ColorConsole.WriteSuccess($"{Environment.NewLine}Settings.json file is valid");
            }
        }

        if (!string.IsNullOrWhiteSpace(o.FortressName))
        {
            var result = WriteFortressName.Write(o.FortressName);
            
            if (result)
            {
                ColorConsole.WriteSuccess($"Fortress name updated to {o.FortressName}");
            }
            else
            {
                ColorConsole.WriteError("Fortress name was not updated.");   
            }
        }
        
        if (o.List)
        {
            var fortressName = GetFortressJsonName.Get();
            ColorConsole.WriteInfo($"Fortress name: {fortressName}");
        }

        if (o.CreateBackup)
        {
            try
            {

                if (GenerateBackup.Run(o.BackupName ?? string.Empty))
                {
                    ColorConsole.WriteSuccess("Backup created");
                }
                else
                {
                    ColorConsole.WriteError("Error creating backup");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception: {ex.Message}");
            }
        }
    });

public class Options
{
    [Option('a', "about", Required = false, HelpText = "About Dwarf Fortress Backup")]
    public bool About { get; set; }
    [Option(shortName: 'n', longName: "name", Required = false, HelpText = "Update Fortress name")]
    public string? FortressName { get; set; }
    [Option('c', "clean", Required = false, HelpText = "Delete saves older than 7 days")]
    public bool Clean { get; set; }
    [Option('v', "validate", Required = false, HelpText = "Validate settings.json file")]
    public bool Validate { get; set; }
    [Option('l', "list", Required = false, HelpText = "Current Fortress name")]
    public bool List { get; set; }
    [Option('b', "backup", Required = false, HelpText = "Create new backup")]
    public bool CreateBackup { get; set; }
    public string? BackupName { get; set; }
    [Option('r', "restore", Required = false, HelpText = "Restore most recent backup")]
    public bool Restore { get; set; }
}
