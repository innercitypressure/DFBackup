using CommandLine;
using DFBackup.Runner.Application;

ColorConsole.WriteWrappedHeader("Dwarf Fortress Backup Manager");

Parser.Default.ParseArguments<Options>(args)
    .WithParsed<Options>(o =>
    {
        if (o.About)
        {
            Console.WriteLine($"Dwarf Fortress Backup Version {GetAssemblyVersion.Display()}");
            Console.WriteLine("-- Repo:");
        }

        if (o.Validate)
        {
            Console.WriteLine("Validating settings.json....");

            var validationResults = ValidateSettings.Check();

            foreach (var result in validationResults.ValidationResults)
            {
                Console.WriteLine(result);
            }

            if (!validationResults.ValidFile)
            {
                Console.WriteLine($"{Environment.NewLine}Settings.json file is NOT valid.");
            }
            else
            {
                Console.WriteLine($"{Environment.NewLine}Settings.json file is valid");
            }
        }

        if (!string.IsNullOrWhiteSpace(o.FortressName))
        {
            var result = WriteFortressName.Write(o.FortressName);
            if (result)
            {
                Console.WriteLine($"Fortress name updated to {o.FortressName}");
            }
            else
            {
                Console.WriteLine("Fortress name was not updated.");   
            }
        }
        
        if (o.List)
        {
            var fortressName = GetFortressJsonName.Get();
            Console.WriteLine($"Fortress name found: {fortressName}");
        }

        if (o.CreateBackup)
        {
            if (GenerateBackup.Run())
            {
                Console.WriteLine("Backup created"); 
            }
            else
            {
                Console.WriteLine("Error creating backup");
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
    [Option('r', "restore", Required = false, HelpText = "Restore most recent backup")]
    public bool Restore { get; set; }
}
