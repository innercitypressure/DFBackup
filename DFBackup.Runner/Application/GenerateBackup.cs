using System.Text.Json;
using DFBackup.Runner.Application.Services;
using DFBackup.Runner.Models;

namespace DFBackup.Runner.Application;

public static class GenerateBackup
{
    public static bool Run(string backupName = "")
    {
        var fortressName = GetFortressJsonName.Get() ?? string.Empty;
        var settings = GetSettingsJson.Get() ?? null;

        if (string.IsNullOrEmpty(backupName))
        {
            backupName = string.Empty;
        }
        
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
        var backupTime = $"{DateTime.Now:yyyyMMdd_hhmmss}";
        
        if (!string.IsNullOrEmpty(fortressName))
        {
            var attemptedDestination = $"{settings.Destination}\\{fortressName}\\{backupTime}";

            if (!string.IsNullOrWhiteSpace(backupName))
            {
                var parsedBackupName = CleanDirectoryNameInput.Clean(backupName);
                
                attemptedDestination += $"_{parsedBackupName}";
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
            destination = settings.Destination + $"\\{backupTime}";
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
