using System.Data;

namespace tfpcollab.userspace.infrastructure.database;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}