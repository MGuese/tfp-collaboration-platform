using tfp_collab_userspace_domain.DomainObjects;
using tfp_collab_userspace_storage_database.Model;

namespace tfp_collab_userspace_storage_database.Mapping;

public static class GalleryMappingExtensions
{
    // Mapping from ImageModel to ImageDto
    public static GalleryDo? ToDo(this Gallery? model)
    {
        if (model == null) return null;

        return new GalleryDo()
        {
            Id = (GalleryId)model.Id, 
            Name = model.Name, 
            OwnerId = (OwnerId)model.OwnerId,
            AddedOn = model.AddedOn
        };
    }

    // Mapping from ImageDto to ImageModel
    public static Gallery? ToModel(this GalleryDo? dto)
    {
        if (dto == null) return null;

        return new Gallery()
        {
            Id = dto.Id, 
            Name = dto.Name, 
            OwnerId = dto.OwnerId, 
            AddedOn = dto.AddedOn
        };
    }
}