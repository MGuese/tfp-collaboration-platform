using tfpcollab.userspace.infrastructure.database.Initialize;

namespace tfpcollab.userspace.api;

public static class HostExtensions
{
    public static async Task InitializeDatabaseAsync(this IHost host)
    {
        // Erstelle einen Scope, um Dienste zu erhalten, die Scoped sind (wie DbContexts)
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            
            // Hole den initialisierenden Dienst
            var databaseInitializer = services.GetRequiredService<IDatabaseInitializer>();
            
            // Führe die Initialisierung aus
            await databaseInitializer.InitializeDatabaseAsync();
        }
    }
}