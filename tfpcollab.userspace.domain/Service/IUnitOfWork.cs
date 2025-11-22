namespace tfpcollab.userspace.domain.Service;

public interface IUnitOfWork
    : IDisposable
{
    IGalleryRepository GalleryRepository { get; set; }
    Task SaveAsync();
}