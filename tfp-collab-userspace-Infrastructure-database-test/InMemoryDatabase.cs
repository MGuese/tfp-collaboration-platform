using System.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;

namespace tfp_collab_userspace_storage_database_test;

public class InMemoryDatabase
{
    private readonly OrmLiteConnectionFactory _dbFactory = 
        new OrmLiteConnectionFactory(":memory:", SqliteOrmLiteDialectProvider.Instance);

    public InMemoryDatabase()
    {
        var connection = OpenConnection();
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
        return this._dbFactory.OpenDbConnection();
    }
    
    public void Insert<T>(IEnumerable<T> items)
    {
        using var db = this.OpenConnection();
        //db.CreateTableIfNotExists<T>();
        foreach (var item in items)
        {
            db.Insert(item);
        }
    }
}