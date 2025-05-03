using System.Data;
using Dapper;
using FluentResults;
using tfp_collab_userspace_storage_database.Model;

namespace tfp_collab_userspace_storage_database;

public class GalleryRepository
    : IGalleryRepository
{
    private readonly IDbConnection _connection;

    public GalleryRepository(IDatabaseConnectionFactory connectionFactory)
    {
        _connection = connectionFactory.GetConnection();
    }

    public async Task<Result<Gallery>> GetByIdAsync(Guid id)
    {
        var gallery = 
            await _connection.QuerySingleOrDefaultAsync<Gallery>(
                "SELECT * FROM Gallery WHERE Id = @Id", 
                new { Id = id });
        
        if (gallery is null) return Result.Fail("Gallery cannot be selected.");
        return Result.Ok(gallery);
    }

    public async Task<Result> CreateAsync(Gallery gallery)
    {
        gallery.Id = Guid.NewGuid();
        gallery.AddedOn = DateTime.UtcNow;
        var rowsInserted = await _connection.ExecuteAsync(
            "INSERT INTO Gallery (Id, OwnerId, Name, AddedOn) VALUES (@Id, @OwnerId, @Name, @AddedOn)", 
            gallery);
        
        if (rowsInserted <= 0) return Result.Fail("Gallery cannot be created.");
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var rowsDeleted = await _connection.ExecuteAsync(
            "DELETE FROM Gallery WHERE Id = @Id", 
            new { Id = id });
        
        if (rowsDeleted <= 0) return Result.Fail("Gallery cannot be deleted.");
        return Result.Ok();
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}