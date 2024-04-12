namespace web_tabel.Domain;

/// <summary>
/// Должность
/// </summary>
public class Position : Entity
{
    private string _name;
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
    
    private Department _department;
    public Department Department
    {
        get => _department;
        protected set => _department = value;
    }

    public Position(string name, Organization organization, Department department)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _organization = organization?? throw new ArgumentNullException(nameof(organization));
        _department = department?? throw new ArgumentNullException(nameof(department));
    }
    
    public Position() {}
}