namespace web_tabel.Domain;

/// <summary>
/// Вид рабочего времени
/// </summary>
public class TypeEmployment
{
    public string Name { get; set; }

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

    public override string ToString()
    {
        return Name;
    }

    public static TypeEmployment GetWorkType()
    {
        return new TypeEmployment() { Name = "РВ", Description = "Рабочее время" };
    }

    public static TypeEmployment GetWeekend()
    {
        return new TypeEmployment() { Name = "ВХ", Description = "Выходной" };
    }
}