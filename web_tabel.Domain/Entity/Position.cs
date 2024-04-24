namespace web_tabel.Domain;

/// <summary>
/// Должность
/// </summary>
public class Position : Entity
{
    public string Name { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual Department Department { get; set; }

    public Position(string name, Organization organization, Department department)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Organization = organization ?? throw new ArgumentNullException(nameof(organization));
        Department = department ?? throw new ArgumentNullException(nameof(department));
    }

    public Position() { }
}