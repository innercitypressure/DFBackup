using System.Text;
using System.Text.Json;
using DFBackup.Runner.Application.Services;
using DFBackup.Runner.Models;

namespace DFBackup.Runner.Application;

public static class WriteFortressName
{
    public static bool Write(string fortressName)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
        
        var strippedFortressName = CleanDirectoryNameInput.Clean(fortressName);

        var jsonContents = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}settings.json");

        var settingsJson = JsonSerializer.Deserialize<Settings>(jsonContents) ?? new Settings();

        settingsJson.FortressName = strippedFortressName;
        
        var jsonString = JsonSerializer.Serialize(settingsJson, jsonOptions);

        try
        {
            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}settings.json", jsonString);
        }
        catch (Exception e)
        {
            ColorConsole.WriteError($"Exception: {e.Message}");
            return false;
        }
        
        return true;
    }
}
