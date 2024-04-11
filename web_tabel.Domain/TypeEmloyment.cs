namespace web_tabel.Domain;

/// <summary>
/// Вид занятости
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