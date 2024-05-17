namespace web_tabel.Domain;

/// <summary>
/// Имя сотрудника
/// </summary>
public class EmployeeName : Entity
{
    #region Свойства

    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    public string MiddleName { get; set; }
    #endregion

    /// <summary>
    /// ФИО
    /// </summary>
    public string FullName => $"{LastName} {FirstName} {MiddleName}";

    ///  <summary>
    /// Инициалы
    /// </summary>
    public string InitialsName => $"{LastName} {FirstName.Substring(0, 1).ToUpper()}. {MiddleName.Substring(0, 1).ToUpper()}.";

    public EmployeeName(string fullName)
    {
        var names = fullName.Split(' ');
        FirstName = names[1] ?? throw new ArgumentException("Фамилия не может быть пустой");
        LastName = names[0] ?? throw new ArgumentException("Имя не может быть пустым");
        MiddleName = names[2] ?? throw new ArgumentException("Отчество не может быть пустым");
    }

    public EmployeeName(string firstName, string lastName, string middleName) 
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    public EmployeeName() { }
}