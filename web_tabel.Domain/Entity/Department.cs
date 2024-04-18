namespace web_tabel.Domain;

/// <summary>
/// Подразделение
/// </summary>
public class Department : Entity
{

    public Department(string name, Organization organization)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Organization = organization?? throw new ArgumentNullException(nameof(organization));
    }

    public string Name { get; set; }

    public Organization Organization {  get; set; }
    
    public Department() {}
}