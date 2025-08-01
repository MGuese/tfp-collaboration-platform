using System.Data;
using Microsoft.Extensions.Logging;
using tfp_collab_userspace_domain.Service;

namespace tfp_collab_userspace_storage_database.Repositories;

public class UnitOfWork
    : IUnitOfWork
{
    private IDbTransaction? _dbTransaction;
    private readonly ILogger<IUnitOfWork> _logger;

    public UnitOfWork(IDbConnectionFactory dbFactory, ILogger<IUnitOfWork> logger)
    {
        _dbTransaction = dbFactory.CreateConnection().BeginTransaction();
        GalleryRepository = new GalleryRepository(_dbTransaction);
        _logger = logger;
    }
    
    public IGalleryRepository GalleryRepository { get; set; }
    public Task SaveAsync()
    {
        _dbTransaction.Commit();
        _logger.LogDebug("Saving changes to Database");
        _dbTransaction.Connection?.Close();
        _logger.LogDebug("connection closed");
        _dbTransaction = null;
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _dbTransaction?.Dispose();
        _logger.LogDebug("Saving changes to Database");
        _dbTransaction =  null;
    }
}