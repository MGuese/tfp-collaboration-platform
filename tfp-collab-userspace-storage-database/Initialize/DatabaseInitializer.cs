using Dapper;
using Microsoft.Extensions.Logging;

namespace tfp_collab_userspace_storage_database.Initialize;

public class DatabaseInitializer 
    : IDatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory; // Deine Implementierung der Verbindungsfactory
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory, ILogger<DatabaseInitializer> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task InitializeDatabaseAsync()
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            // Tabelle für Gallery erstellen
            await connection.ExecuteAsync(@"
                    CREATE TABLE IF NOT EXISTS Gallery (
                        Id UUID PRIMARY KEY,
                        OwnerId UUID NOT NULL,
                        Name TEXT,
                        AddedOn TIMESTAMP
                    );
                ");
                
            connection.Close();
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Failed to initialize database");
            throw;
        }
    }
}