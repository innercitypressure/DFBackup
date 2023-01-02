using System.Text;
using System.Text.Json;
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

        HashSet<char> removeChars = new HashSet<char>(" ?&^$#@!()+-,:;<>’\'-_*");
        StringBuilder result = new StringBuilder(fortressName.Length);
        foreach (char c in fortressName)
            if (!removeChars.Contains(c)) // prevent dirty chars
                result.Append(c);
      
        var strippedFortressName = result.ToString();

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
