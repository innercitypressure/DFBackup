using System.Text;
using System.Text.Json;
using System.Text.Unicode;

namespace DFBackup.Runner.Application;

public static class WriteFortressName
{
    public static bool Write(string fortressName)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        HashSet<char> removeChars = new HashSet<char>(" ?&^$#@!()+-,:;<>’\'-_*");
        StringBuilder result = new StringBuilder(fortressName.Length);
        foreach (char c in fortressName)
            if (!removeChars.Contains(c)) // prevent dirty chars
                result.Append(c);
        var strippedFortressName = result.ToString();
        
        var fortressJson = new Fortress
        {
            FortressName = strippedFortressName ?? ""
        };

        var jsonString = JsonSerializer.Serialize(fortressJson, jsonOptions);

        try
        {
            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}fortress.json", jsonString);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            return false;
        }
        
        return true;
    }
}