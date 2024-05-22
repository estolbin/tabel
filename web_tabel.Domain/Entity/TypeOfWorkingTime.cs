using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace web_tabel.Domain;

/// <summary>
/// Вид рабочего времени
/// </summary>
public class TypeOfWorkingTime
{
    public string Name { get; set; }

    public TypeOfWorkingTime(string name)
    {
        Name = name ?? throw new ArgumentNullException("Name can't be empty");
    }

    public TypeOfWorkingTime() { }

    [JsonIgnore]
    [MaxLength(7)]
    public string? ColorText { get; set; }

    public TypeOfWorkingTime(string name, string description) : this(name)
    {
        Description = description ?? throw new ArgumentNullException("Description can't be empty");
    }

    public string Description { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;

        if (obj is TypeOfWorkingTime other)
        {
            return this.Name == other.Name;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }

    public static TypeOfWorkingTime GetWorkType()
    {
        return new TypeOfWorkingTime() { Name = "РВ", Description = "Рабочее время" };
    }

    public static TypeOfWorkingTime GetWeekend()
    {
        return new TypeOfWorkingTime() { Name = "ВХ", Description = "Выходной" };
    }
}