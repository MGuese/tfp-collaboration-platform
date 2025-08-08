using System.Data;
using Dapper;
using FluentResults;
using tfp_collab_userspace_domain.DomainObjects;
using tfp_collab_userspace_domain.Service;
using tfp_collab_userspace_storage_database.Mapping;
using tfp_collab_userspace_storage_database.Model;

namespace tfp_collab_userspace_storage_database.Repositories;

public class GalleryRepository
    : IGalleryRepository
{
    private readonly IDbTransaction _transaction;

    public GalleryRepository(IDbTransaction connection)
    {
        _transaction = connection;
    }

    public async Task<Result<GalleryDo>> GetAsync(OwnerId ownerId, GalleryId galleryId)
    {
        if (_transaction.Connection is null) throw new InvalidOperationException("Connection is null.");
        
        try
        {
            var gallery = 
                await _transaction.Connection.QuerySingleOrDefaultAsync<Gallery>(
                    "SELECT * FROM Gallery WHERE OwnerId = @OwnerId and Id = @Id", 
                    new { Id = galleryId.Value, OwnerId =  ownerId.Value });
            
            return gallery is null ? Result.Fail("Gallery has not been selected.") : Result.Ok(gallery.ToDo());
        }
        catch (Exception ex)
        {
            return Result.Fail("Gallery has not been selected.").WithError(ex.ToString());
        }
    }
    
    public async Task<Result<IEnumerable<GalleryDo>>> GetAsync(OwnerId ownerId)
    {
        if (_transaction.Connection is null) throw new InvalidOperationException("Connection is null.");
        
        try
        {
            var galleries = 
                await _transaction.Connection
                    .QueryAsync<Gallery>("SELECT * FROM Gallery WHERE OwnerId = @ownerId and");
            
            return !galleries.Any() ? Result.Fail("Galleries cannot be selected.") : Result.Ok(galleries.Select(g => g.ToDo()));
        }
        catch (Exception ex)
        {
            return Result.Fail("Gallery cannot be selected.").WithError(ex.ToString());
        }
    }

    public async Task<Result<GalleryDo>> CreateAsync(GalleryDo galleryDo)
    {
        if (_transaction.Connection is null) throw new InvalidOperationException("Connection is null.");
        try
        {
            var gallery = galleryDo.ToModel();
            
            gallery.Id = GalleryId.New();
            gallery.AddedOn = DateTime.UtcNow;
            var rowsInserted = 
                await _transaction.Connection.ExecuteAsync(
                    "INSERT INTO Gallery (Id, OwnerId, Name, AddedOn) VALUES (@Id, @OwnerId, @Name, @AddedOn)", 
                    gallery);
            
            if (rowsInserted <= 0) return Result.Fail("Gallery has not been created.");
            galleryDo = gallery.ToDo();
            return Result.Ok(galleryDo);
        }
        catch (Exception ex)
        {
            return Result.Fail("Gallery has not been created.").WithError(ex.ToString());
        }
    }

    public async Task<Result> DeleteAsync(GalleryId id)
    {
        if (_transaction.Connection is null) throw new InvalidOperationException("Connection is null.");
        
        try
        {
            var rowsDeleted = 
                await _transaction.Connection.ExecuteAsync(
                    "DELETE FROM Gallery WHERE Id = @Id", 
                    new { Id = id.Value });
            
            return rowsDeleted <= 0 ? Result.Fail("Gallery has not been deleted.") : Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail("Gallery has not been deleted.").WithError(ex.ToString());
        }
    }
}