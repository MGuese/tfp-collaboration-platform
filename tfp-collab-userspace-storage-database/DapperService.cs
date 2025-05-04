using tfp_collab_userspace_domain.dto;
using tfp_collab_userspace_domain.Service;
using tfp_collab_userspace_storage_database.Mapping;
using tfp_collab_userspace_storage_database.Repositories;

namespace tfp_collab_userspace_storage_database;

public class DapperService 
    : IDatabaseService
{
    IGalleryRepository _galleryRepository;
    public DapperService(IGalleryRepository imageRepository)
    {
        _galleryRepository = imageRepository;
    }

    public async Task<GalleryDto?> GetGalleryByIdAsync(Guid id)
    {
        var  imageModelResult = 
            await _galleryRepository.GetByIdAsync(id);

        if (imageModelResult.IsSuccess)
        {
            // Todo Logging
            return imageModelResult.Value.ToDto();;
        }

        return null;
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