using System.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;

namespace tfpcollab.userspace.Infrastructure.database.test;

public class InMemoryDatabase
{
    private readonly OrmLiteConnectionFactory _dbFactory = 
        new OrmLiteConnectionFactory(":memory:", SqliteOrmLiteDialectProvider.Instance);

    public InMemoryDatabase()
    {
        var connection = OpenConnection();
        CreateDbSchema(connection);
    }

    private static void CreateDbSchema(IDbConnection connection)
    {
        connection.ExecuteSql(
            @"PRAGMA foreign_keys = ON;
                CREATE TABLE Gallery 
                (
                    Id BLOB PRIMARY KEY,
                    OwnerId BLOB,
                    Name TEXT,
                    AddedOn TIMESTAMP,
                    UNIQUE (OwnerId, Name)
                );");
    }

    public IDbConnection OpenConnection()
    {
        return _dbFactory.OpenDbConnection();
    }
    
    public void Insert<T>(IEnumerable<T> items)
    {
        using var db = OpenConnection();
        foreach (var item in items)
        {
            db.Insert(item);
        }
    }
}