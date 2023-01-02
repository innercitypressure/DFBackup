using System.Text.Json;
using DFBackup.Runner.Models;

namespace DFBackup.Runner.Application;

public class GetSettingsJson
{
    public static Settings Get()
    {
        var settingsJson = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}settings.json");
        var settings = JsonSerializer.Deserialize<Settings>(settingsJson) ?? new Settings();

        return settings;
    }
}
