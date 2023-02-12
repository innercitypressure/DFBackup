using System.Text;

namespace DFBackup.Runner.Application.Services;

public static class CleanDirectoryNameInput
{
    public static string Clean(string input)
    {
        HashSet<char> removeChars = new HashSet<char>(" ?&^$#@!()+-,:;<>’\'-_*");
        StringBuilder results = new StringBuilder(input.Length);
        foreach (char c in input)
            if (!removeChars.Contains(c)) // prevent dirty chars
                results.Append(c);

        return results.ToString();
    }
}