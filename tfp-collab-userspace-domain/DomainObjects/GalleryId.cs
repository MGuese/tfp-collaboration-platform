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

    // Expliziter Umwandlungsoperator von Guid nach GalleryId
    public static explicit operator GalleryId(Guid guid) => new(guid);

    // Expliziter Umwandlungsoperator von string nach GalleryId
    public static explicit operator GalleryId(string guid) => new(Guid.Parse(guid));

    // Impliziter Umwandlungsoperator von GalleryId nach Guid (optional, aber nützlich)
    public static implicit operator Guid(GalleryId id) => id.Value;
    
    public override bool Equals(object? obj)
    {
        return obj is GalleryId other && Equals(other);
    }

    public bool Equals(GalleryId other)
    {
        return EqualityComparer<Guid>.Default.Equals(Value, other.Value);
    }

    public static bool operator ==(GalleryId left, GalleryId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(GalleryId left, GalleryId right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<Guid>.Default.GetHashCode(Value);
    }
}