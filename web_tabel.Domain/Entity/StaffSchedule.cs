namespace web_tabel.Domain;

/// <summary>
/// Штатное расписание
/// </summary>
public class StaffSchedule : Entity
{
    // aggregate root
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

    private Position _position;

    public Position Position
    {
        get => _position;
        protected set => _position = value;
    }

    private Guid? _parentId;
    public Guid? ParentId { get => _parentId; set => _parentId = value;}
    
    private string _name;
    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get => _name; set => _name = value;}

    private float _numberOfPositions;
    /// <summary>
    /// Количество единиц
    /// </summary>
    public float NumberOfPositions
    {
        get => _numberOfPositions; 
    }

    public void SetNumberOfPositions(float number)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(number);
        _numberOfPositions = number;
    }
    
     private WorkSchedule _workSchedule;

     public WorkSchedule WorkSchedule
     {
         get => _workSchedule; 
         protected set => _workSchedule = value; 
     }

    public StaffSchedule(Organization organization, Department department, Position position, WorkSchedule workSchedule, string name)
    {
        _organization = organization ?? throw new ArgumentNullException(nameof(organization));
        _department = department ?? throw new ArgumentNullException(nameof(department));
        _position = position ?? throw new ArgumentNullException(nameof(position));
        _workSchedule = workSchedule?? throw new ArgumentNullException(nameof(workSchedule));
        _name = name ?? throw new ArgumentNullException(nameof(name));
    }
    
    public StaffSchedule() {}


}