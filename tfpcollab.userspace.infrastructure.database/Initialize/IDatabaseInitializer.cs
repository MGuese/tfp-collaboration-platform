namespace tfpcollab.userspace.infrastructure.database.Initialize;

public interface IDatabaseInitializer
{
    Task InitializeDatabaseAsync();
}