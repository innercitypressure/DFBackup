using DFBackup.Runner.Models;

namespace DFBackup.Runner.Application;

public static class GenerateBackup
{
    public static bool Run()
    {
        var fortressName = GetFortressJsonName.Get() ?? null;
        var settings = GetSettingsJson.Get() ?? null;

        if (settings == null)
        {
            Console.WriteLine("No settings.json file could be found/parsed");
            return false;
        }


        var destination = "";
        
        if (!string.IsNullOrEmpty(fortressName))
        {
            var checkFolder = Directory.Exists($"{settings.Destination}\\{fortressName}");

            if (checkFolder == false)
            {
                Directory.CreateDirectory($"{settings.Destination}\\{fortressName}");
            }

            destination = $"{settings.Destination}\\{fortressName}";
        }
        else
        {
            destination = settings.Destination;
        }

        try
        {
            foreach (var directory in Directory.GetDirectories(settings.Source))
            {
                CloneDirectory(settings.Source, destination);
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error creating backup {ex.Message}");
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

