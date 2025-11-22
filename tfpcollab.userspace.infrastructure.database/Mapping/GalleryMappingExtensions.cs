using tfpcollab.userspace.domain.DomainObjects;
using tfpcollab.userspace.infrastructure.database.Model;

namespace tfpcollab.userspace.infrastructure.database.Mapping;

public static class GalleryMappingExtensions
{
    // Mapping from ImageModel to ImageDto
    public static GalleryDo ToDo(this Gallery model)
    {
        return new GalleryDo
        {
            Id = model.Id, 
            Name = model.Name, 
            OwnerId = model.OwnerId,
            AddedOn = model.AddedOn
        };
    }

    // Mapping from ImageDto to ImageModel
    public static Gallery ToModel(this GalleryDo dto)
    {
        return new Gallery
        {
            Id = dto.Id, 
            Name = dto.Name, 
            OwnerId = dto.OwnerId, 
            AddedOn = dto.AddedOn
        };
    }
}