using System.Reflection;

namespace DFBackup.Runner.Application;

public static class GetAssemblyVersion
{
        public static string? Display()
        {
            return Assembly.GetEntryAssembly()?.GetName().Version?.ToString();
        }
}
