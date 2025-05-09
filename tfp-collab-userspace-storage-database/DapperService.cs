using Microsoft.Extensions.Logging;
using FluentResults;
using tfp_collab_userspace_domain.dto;
using tfp_collab_userspace_domain.Service;
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

    public async Task<GalleryDto?> Get(Guid id)
    {
        var  imageModelResult = await _galleryRepository.Get(id);

        if (imageModelResult.IsSuccess)
        {
            // Todo Logging
            return imageModelResult.Value.ToDto();;
        }
        else
        {
            _logger.LogFluentResultErrors<DapperService, Gallery>(imageModelResult, "");
        }

        return null;
    }
    
    public async Task<IEnumerable<GalleryDto>> Get()
    {
        var  imageModelResult = await _galleryRepository.Get();

        if (imageModelResult.IsSuccess)
        {
            // Todo Logging
            return imageModelResult.Value.Select(model => model.ToDto());
        }

        return default;
    }

    public async Task CreateGalleryAsync(GalleryDto galleryDto)
    {
        var model = galleryDto.ToModel();
        await _galleryRepository.CreateAsync(model);
        galleryDto.Id = model.Id;
        galleryDto.AddedOn = model.AddedOn;
    }
    
    public async Task DeleteGalleryAsync(Guid id)
    {
        await _galleryRepository.DeleteAsync(id);
    }
}