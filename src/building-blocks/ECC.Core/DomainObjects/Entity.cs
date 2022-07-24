using ECC.Core.Messages;

namespace ECC.Core.DomainObjects;

public abstract class Entity
{
    public Guid Id { get; set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    private List<Event> _notifications;


    public IReadOnlyCollection<Event> Notifications => _notifications?.AsReadOnly();

    public void AddEvent(Event e)
    {
        _notifications = _notifications ?? new List<Event>();
        _notifications.Add(e);
    }

    public void RemoveEvent(Event e)
    {
        _notifications?.Remove(e);
    }
    public void CleanEvents()
    {
        _notifications?.Clear();
    }

    #region comparision
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
    #endregion


}