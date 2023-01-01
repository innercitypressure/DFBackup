using System.Text.Json;
using DFBackup.Runner.Models;

namespace DFBackup.Runner.Application;

public static class ValidateSettings
{
    public static ValidateSettingsResult Check()
    {
        var validationSettingsResult = new ValidateSettingsResult();

        if (!File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}settings.json"))
        {
            validationSettingsResult.ValidationResults.Add("settings.json not found!");
            validationSettingsResult.ValidFile = false;
        }
        
        var settingsJson = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}settings.json");
        var settings = JsonSerializer.Deserialize<Settings>(settingsJson) ?? null;
        
        if (settings == null)
        {
            validationSettingsResult.ValidationResults.Add("settings.json file is malformed.");
            validationSettingsResult.ValidFile = false;
        }
        else
        {
            validationSettingsResult.ValidationResults.Add("settings.json file appears valid");
        }

        if (!Directory.Exists(settings?.Destination))
        {
            validationSettingsResult.ValidationResults.Add($"Destination directory {settings?.Destination} was NOT found.");
            validationSettingsResult.ValidFile = false;
        }
        else
        {
            validationSettingsResult.ValidationResults.Add($"Directory {settings?.Destination} was found.");
        }

        if (!Directory.Exists(settings?.Source))
        {
            validationSettingsResult.ValidationResults.Add($"Source directory {settings?.Source} was NOT found.");
            validationSettingsResult.ValidFile = false;
        }
        else
        {
            validationSettingsResult.ValidationResults.Add($"Source directory {settings?.Source} was found.");
        }

        return validationSettingsResult;
    }
}