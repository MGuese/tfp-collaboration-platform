using System.Data;
using Dapper;
using FluentResults;
using tfp_collab_userspace_storage_database.Model;

namespace tfp_collab_userspace_storage_database.Repositories;

public class GalleryRepository
    : IGalleryRepository
{
    private readonly IDbConnection _connection;

    public GalleryRepository(IDbConnectionFactory connectionFactory)
    {
        _connection = connectionFactory.CreateConnection();
    }

    public async Task<Result<Gallery>> Get(Guid ownerId, Guid galleryId)
    {
        try
        {
            var gallery = 
                await _connection.QuerySingleOrDefaultAsync<Gallery>(
                    "SELECT * FROM Gallery WHERE OwnerId = @OwnerId and Id = @Id", 
                    new { Id = galleryId, OwnerId =  ownerId });
            
            if (gallery is null) return Result.Fail("Gallery has not been selected.");
            return Result.Ok(gallery);
        }
        catch (Exception ex)
        {
            return Result.Fail("Gallery has not been selected.").WithError(ex.ToString());
        }
    }
    
    public async Task<Result<IEnumerable<Gallery>>> Get(Guid ownerId)
    {
        try
        {
            var galleries = await _connection.QueryAsync<Gallery>("SELECT * FROM Gallery WHERE OwnerId = @ownerId and");
            
            if (galleries is null) return Result.Fail("Galleries cannot be selected.");
            return Result.Ok(galleries);
        }
        catch (Exception ex)
        {
            return Result.Fail("Gallery cannot be selected.").WithError(ex.ToString());
        }
    }

    public async Task<Result> CreateAsync(Gallery gallery)
    {
        try
        {
            gallery.Id = Guid.NewGuid();
            gallery.AddedOn = DateTime.UtcNow;
            var rowsInserted = await _connection.ExecuteAsync(
                "INSERT INTO Gallery (Id, OwnerId, Name, AddedOn) VALUES (@Id, @OwnerId, @Name, @AddedOn)", 
                gallery);
            
            if (rowsInserted <= 0) return Result.Fail("Gallery has not been created.");
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail("Gallery has not been created.").WithError(ex.ToString());
        }
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        try
        {
            var rowsDeleted = await _connection.ExecuteAsync(
                "DELETE FROM Gallery WHERE Id = @Id", 
                new { Id = id });
            
            if (rowsDeleted <= 0) return Result.Fail("Gallery has not been deleted.");
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail("Gallery has not been deleted.").WithError(ex.ToString());
        }
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}