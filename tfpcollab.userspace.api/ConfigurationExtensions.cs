namespace tfpcollab.userspace.api;

public static class ConfigurationExtensions
{
    // Statische Methode, die die konfigurierte IConfiguration zurückgibt
    public static IConfiguration BuildConfiguration()
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        if (string.IsNullOrEmpty(environmentName))
        {
            // Wenn keine Umgebungsvariable gesetzt ist, wird standardmäßig 'Development' angenommen.
            // Dies ist eine gängige Konvention für lokale Entwicklung.
            environmentName = "Development";
            Console.WriteLine("Umgebungsvariable ASPNETCORE_ENVIRONMENT/DOTNET_ENVIRONMENT nicht gefunden. Standard auf 'Development' gesetzt.");
        }

        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            // 2. Lade umgebungsspezifische Datei (z.B. appsettings.Development.json oder appsettings.Production.json)
            //    Diese Datei überschreibt bei Übereinstimmung Werte aus appsettings.json
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            // Optional: Umgebungsvariablen können alles andere überschreiben
            .AddEnvironmentVariables()
            .Build();
    }

}