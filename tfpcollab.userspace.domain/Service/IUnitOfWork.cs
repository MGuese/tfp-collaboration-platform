namespace tfp_collab_userspace_domain.Service;

public interface IUnitOfWork
    : IDisposable
{
    IGalleryRepository GalleryRepository { get; set; }
    Task SaveAsync();
}