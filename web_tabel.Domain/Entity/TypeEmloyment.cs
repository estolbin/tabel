namespace web_tabel.Domain;

/// <summary>
/// Вид рабочего времени
/// </summary>
public class TypeEmployment
{
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException("Name can't be empty");
            if (value.Length > 3) throw new ArgumentOutOfRangeException("Name can't be more than 3 characters");
            _name = value;
        } 
        
    }

    public TypeEmployment(string name)
    {
        Name = name ?? throw new ArgumentNullException("Name can't be empty");
    }
    
    public TypeEmployment() {}

    public TypeEmployment(string name, string description) : this(name)
    {
        Description = description?? throw new ArgumentNullException("Description can't be empty");
    }
    
    public string Description { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        
        if (obj is TypeEmployment other)
        {
            return this.Name == other.Name;
        }
        return false;
    }
}