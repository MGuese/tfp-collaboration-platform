using Microsoft.Extensions.Logging;
using tfp_collab_userspace_domain.Service;
using tfp_collab_userspace_interfaces.dto;
using tfp_collab_userspace_storage_database.Mapping;
using tfp_collab_userspace_storage_database.Model;
using tfp_collab_userspace_storage_database.Repositories;

namespace tfp_collab_userspace_storage_database;

public class DapperService 
    : IDatabaseService
{
    IGalleryRepository _galleryRepository;
    private readonly ILogger<DapperService> _logger;

    public DapperService(IGalleryRepository imageRepository, ILogger<DapperService> logger)
    {
        _galleryRepository = imageRepository;
        _logger = logger;
    }

    public async Task<GalleryDto?> GetAsync(Guid ownerId, Guid galleryId)
    {
        var  imageModelResult = await _galleryRepository.Get(ownerId, galleryId);

        if (imageModelResult.IsSuccess)
        {
            // Todo Logging
            return imageModelResult.Value.ToDto();;
        }
        else
        {
            _logger.LogFluentResultErrors<DapperService, Gallery>(imageModelResult, "Get Gallery by Id");
        }

        return null;
    }
    
    public async Task<IEnumerable<GalleryDto>> GetAsync(Guid ownerId)
    {
        var  imageModelResult = await _galleryRepository.Get(ownerId);

        if (imageModelResult.IsSuccess)
        {
            // Todo Logging
            return imageModelResult.Value.Select(model => model.ToDto());
        }
        else
        {
            _logger.LogFluentResultErrors<DapperService, IEnumerable<Gallery>>(imageModelResult, "Get all Galleries");
        }

        return default;
    }

    public async Task<GalleryDto?> CreateGalleryAsync(GalleryDto galleryDto)
    {
        var model = galleryDto.ToModel();
        var result = await _galleryRepository.CreateAsync(model);

        if (result.IsSuccess)
        {
            galleryDto.Id = model.Id;
            galleryDto.AddedOn = model.AddedOn;
            return galleryDto;
        }
        else
        {
            _logger.LogFluentResultErrors<DapperService, IEnumerable<Gallery>>(result, "Create Gallery");
            return null;
        }
    }
    
    public async Task DeleteGalleryAsync(Guid id)
    {
        var result = await _galleryRepository.DeleteAsync(id);
        
        if (!result.IsSuccess)
        {
            _logger.LogFluentResultErrors<DapperService, IEnumerable<Gallery>>(result, "Delete Gallery");
        }
    }
}