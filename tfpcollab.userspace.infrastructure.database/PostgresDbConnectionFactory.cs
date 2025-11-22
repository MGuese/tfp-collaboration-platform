using System.Data;
using Npgsql;

namespace tfpcollab.userspace.infrastructure.database;

public class PostgresDbConnectionFactory
    : IDbConnectionFactory
{
    private readonly string _connectionString;

    public PostgresDbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}