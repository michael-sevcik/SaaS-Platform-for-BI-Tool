namespace BIManagement.Common.Domain.Primitives;

/// <summary>
/// Represents the abstract entity primitive.
/// </summary>
/// <typeparam name="TEntityId">The entity identifier type.</typeparam>
public abstract class Entity<TEntityId> : IEquatable<Entity<TEntityId>>
    where TEntityId : struct, IEquatable<TEntityId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TEntityId}"/> class.
    /// </summary>
    /// <param name="id">The entity identifier.</param>
    protected Entity(TEntityId id)
        => Id = id;


    /// <summary>
    /// Gets the entity identifier.
    /// </summary>
    public TEntityId Id { get; private init; }

    public static bool operator ==(Entity<TEntityId>? a, Entity<TEntityId>? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TEntityId>? a, Entity<TEntityId>? b) => !(a == b);

    /// <inheritdoc />
    public virtual bool Equals(Entity<TEntityId>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not Entity<TEntityId> other)
        {
            return false;
        }

        return Id.Equals(other.Id);
    }

    /// <inheritdoc />
    public override int GetHashCode() => Id.GetHashCode();
}
