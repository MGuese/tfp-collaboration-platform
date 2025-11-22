using System.Data;
using Dapper;

namespace tfpcollab.userspace.infrastructure.database;

public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override Guid Parse(object value)
    {
        return new Guid((byte[])value);
    }

    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value;
    }
}