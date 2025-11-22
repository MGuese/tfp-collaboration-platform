namespace tfp_collab_userspace_domain.DomainObjects;

public readonly struct GalleryId 
    : IEquatable<GalleryId>
{
    public Guid Value { get; }

    private GalleryId(Guid value)
    {
        Value = value;
    }

    public static GalleryId New() => new(Guid.NewGuid());
    public static implicit operator GalleryId(Guid guid) => new(guid);
    public static implicit operator Guid(GalleryId id) => id.Value;
    
    public override bool Equals(object? obj)
    {
        return obj is GalleryId other && Equals(other);
    }

    public bool Equals(GalleryId other)
    {
        return EqualityComparer<Guid>.Default.Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<Guid>.Default.GetHashCode(Value);
    }
}