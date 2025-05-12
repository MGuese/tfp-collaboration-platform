namespace tfp_collab_userspace_storage_database.Initialize;

public interface IDatabaseInitializer
{
    Task InitializeDatabaseAsync();
}