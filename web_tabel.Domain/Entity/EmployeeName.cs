namespace web_tabel.Domain;

/// <summary>
/// Имя сотрудника
/// </summary>
public class EmployeeName : Entity
{
    #region Свойства

    

    private string _firstName;

    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName
    {
        get => _firstName;
        protected set => _firstName = value;
    }
    
    private string _lastName;

    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName
    {
        get => _lastName;
        protected set => _lastName = value;
    }
    
    private string _middleName;

    /// <summary>
    /// Отчество
    /// </summary>
    public string MiddleName
    {
        get => _middleName;
        protected set => _middleName = value;
    }
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
        _firstName = names[1] ?? throw new ArgumentException("Фамилия не может быть пустой");
        _lastName = names[0]?? throw new ArgumentException("Имя не может быть пустым");
        _middleName = names[2]?? throw new ArgumentException("Отчество не может быть пустым");
    }

    public EmployeeName(string firstName, string lastName, string middleName) : this(lastName + " " + firstName + " " + middleName) {}
}