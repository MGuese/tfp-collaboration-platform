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
    public static implicit operator OwnerId(Guid guid) => new(guid);
    public static explicit operator OwnerId(string guid) => new(Guid.Parse(guid));
    public static implicit operator Guid(OwnerId id) => id.Value;

    public override bool Equals(object? obj)
    {
        return obj is OwnerId other && Equals(other);
    }

    public bool Equals(OwnerId other)
    {
        return EqualityComparer<Guid>.Default.Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<Guid>.Default.GetHashCode(Value);
    }
}