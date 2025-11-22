using tfpcollab.userspace.domain.Request;
using tfpcollab.userspace.domain.Response;

namespace tfpcollab.userspace.domain.UseCase;

public interface ICreateGalleryUseCase
{
    Task<CreateGalleryResponse> Handle(CreateGalleryRequest request);
}