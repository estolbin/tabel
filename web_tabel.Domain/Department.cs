namespace web_tabel.Domain;

/// <summary>
/// Подразделение
/// </summary>
public class Department : Entity
{
    private string _name;

    public Department(string name, Organization organization)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _organization = organization?? throw new ArgumentNullException(nameof(organization));
    }

    public string Name
    {
        get => _name;
        protected set => _name = value;
    }
    
    private Organization _organization;

    public Organization Organization
    {
        get => _organization;
        protected set => _organization = value;
    }
}