using System.Data;

namespace tfp_collab_userspace_storage_database;

public interface IDatabaseConnectionFactory
{
    IDbConnection GetConnection();
}