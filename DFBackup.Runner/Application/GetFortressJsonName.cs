using System.Text.Json;

namespace DFBackup.Runner.Application;

public static class GetFortressJsonName
{
    public static string Get()
    {
        try
        {
            var fortressJson = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}fortress.json");
            var fortress = JsonSerializer.Deserialize<Fortress>(fortressJson) ?? null;

            if (!string.IsNullOrWhiteSpace(fortress?.FortressName))
            {
                return fortress.FortressName;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception finding fortress name {e.Message}");
        }
        
        return "No fortress name was found.";
    }
}
