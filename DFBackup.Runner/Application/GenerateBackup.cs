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

        if (settings.Source == null || settings.Destination == null)
        {
            Console.WriteLine("No source/destination found in settings.json");
            return false;
        }

        var destination = "";
        
        if (!string.IsNullOrEmpty(fortressName))
        {
            var attemptedDestination = $"{settings.Destination}\\{fortressName}\\{DateTime.Now:yyyyMMdd_hh:ss}"; 
            
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
