namespace web_tabel.Domain;

public class Organization : Entity
{
    private string _name;

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public Organization(string name)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
    }
    
    public Organization() {}
}