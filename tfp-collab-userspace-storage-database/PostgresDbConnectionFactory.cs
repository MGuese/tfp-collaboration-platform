using System.Data;
using Npgsql;

namespace tfp_collab_userspace_storage_database;

public class PostgresDbConnectionFactory
    : IDatabaseConnectionFactory
{
    private readonly string _connectionString;

    public PostgresDbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}