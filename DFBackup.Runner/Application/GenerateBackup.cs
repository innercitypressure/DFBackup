using System.Text.Json;
using DFBackup.Runner.Models;

namespace DFBackup.Runner.Application;

public static class GenerateBackup
{
    public static bool Run(string BackupName = "")
    {
        var fortressName = GetFortressJsonName.Get() ?? null;
        var settings = GetSettingsJson.Get() ?? null;

        if (settings == null)
        {
            ColorConsole.WriteError("No settings.json file could be found/parsed");
            return false;
        }

        if (settings.Source == null || settings.Destination == null)
        {
            ColorConsole.WriteError("No source/destination found in settings.json");
            return false;
        }

        var destination = "";
        
        if (!string.IsNullOrEmpty(fortressName))
        {
            var attemptedDestination = $"{settings.Destination}\\{fortressName}\\{DateTime.Now:yyyyMMdd_hhss}";

            if (!string.IsNullOrWhiteSpace(BackupName))
            {
                attemptedDestination += $"-{BackupName}";
            }
            
            var checkFolder = Directory.Exists(attemptedDestination);

            if (checkFolder == false)
            {
                Directory.CreateDirectory(attemptedDestination);
            }

            destination = attemptedDestination;
        }
        else
        {
            destination = settings.Destination + $"\\{DateTime.Now:yyyyMMdd_hh:ss}";
        }

        try
        {
            CloneDirectory(settings.Source, destination);
            
            // Update LastBackupPath
            var jsonContents = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}settings.json");

            var settingsJson = JsonSerializer.Deserialize<Settings>(jsonContents) ?? new Settings();

            settings.LastBackupPath = destination;
            
            var jsonString = JsonSerializer.Serialize(settingsJson, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}settings.json", jsonString);

            return true;
        }
        catch (Exception ex)
        {
            ColorConsole.WriteError($"Error creating backup {ex.Message}");
        }

        return false;
    }
    
    private static void CloneDirectory(string root, string dest)
    {
        foreach (var directory in Directory.GetDirectories(root))
        {
            var newDirectory = Path.Combine(dest, Path.GetFileName(directory));
            Directory.CreateDirectory(newDirectory);
            CloneDirectory(directory, newDirectory);
        }

        foreach (var file in Directory.GetFiles(root))
        {
            File.Copy(file, Path.Combine(dest, Path.GetFileName(file)));
        }
    }
}
