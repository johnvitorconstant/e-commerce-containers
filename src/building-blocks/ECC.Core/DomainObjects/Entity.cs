namespace ECC.Core.DomainObjects;

public abstract class Entity
{
    public Guid Id { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Entity entity &&
               Id.Equals(entity.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    public override string? ToString()
    {
        return $"{GetType().Name} [Id = {Id}]";
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (ReferenceEquals(a, b) && ReferenceEquals(b, null)) return true;

        if (ReferenceEquals(b, null) || ReferenceEquals(b, null)) return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }
}