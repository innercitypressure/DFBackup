using System.Text.Json;
using DFBackup.Runner.Models;

namespace DFBackup.Runner.Application;

public static class GetFortressJsonName
{
    public static string Get()
    {
        try
        {
            var fortressJson = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}settings.json");
            var fortress = JsonSerializer.Deserialize<Settings>(fortressJson) ?? null;

            if (!string.IsNullOrWhiteSpace(fortress?.FortressName))
            {
                return fortress.FortressName;
            }
        }
        catch (Exception e)
        {
            ColorConsole.WriteError($"Exception finding fortress name {e.Message}");
        }
        
        return "No fortress name was found.";
    }
}
