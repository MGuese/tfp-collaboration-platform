using Dapper;

namespace tfp_collab_userspace_storage_database.Initialize;

public class DatabaseInitializer 
    : IDatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory; // Deine Implementierung der Verbindungsfactory

    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeDatabaseAsync()
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
        // Füge hier weitere CREATE TABLE IF NOT EXISTS Anweisungen für deine anderen Modelle hinzu
    }
}