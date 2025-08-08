namespace tfp_collab_userspace_domain.DomainObjects;

public readonly struct OwnerId 
    : IEquatable<OwnerId>
{
    public Guid Value { get; }

    private OwnerId(Guid value)
    {
        Value = value;
    }
    
    public static OwnerId New() => new(Guid.NewGuid());

    // Expliziter Umwandlungsoperator von Guid nach GalleryId
    public static explicit operator OwnerId(Guid guid) => new(guid);

    // Expliziter Umwandlungsoperator von string nach GalleryId
    public static explicit operator OwnerId(string guid) => new(Guid.Parse(guid));

    // Impliziter Umwandlungsoperator von GalleryId nach Guid (optional, aber nützlich)
    public static implicit operator Guid(OwnerId id) => id.Value;

    public override bool Equals(object? obj)
    {
        return obj is OwnerId other && Equals(other);
    }

    public bool Equals(OwnerId other)
    {
        return EqualityComparer<Guid>.Default.Equals(Value, other.Value);
    }

    public static bool operator ==(OwnerId left, OwnerId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(OwnerId left, OwnerId right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<Guid>.Default.GetHashCode(Value);
    }
}