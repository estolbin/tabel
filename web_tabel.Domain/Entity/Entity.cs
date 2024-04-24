namespace web_tabel.Domain;

public abstract class Entity
{
    public virtual Guid Id { get; set; }

    public override bool Equals(object? obj)
    {
        var other = obj as Entity;

        if (ReferenceEquals(other, null)) return false;

        if (ReferenceEquals(this, other)) return true;

        return Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            return true;
        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right) => !(left == right);
}