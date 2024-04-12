namespace web_tabel.Domain;

/// <summary>
/// Сотрудник
/// </summary>
public class Employee : Entity
{
    private EmployeeName _name;

    /// <summary>
    /// Полные фаилия, имя, отчество. Без сокращений.
    /// </summary>
    public EmployeeName Name
    {
        get => (EmployeeName)_name;
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

    /*private Position _position;

    public Position Position
    {
        get => _position;
        protected set => _position = value;
    }*/
    
    private StaffSchedule _staffSchedule;
    /// <summary>
    /// Должность по штатному расписанию.
    /// </summary>
    public StaffSchedule StaffSchedule
    {
        get => _staffSchedule;
        protected set
        {
            _staffSchedule = value;
            if (_workSchedule == null && _staffSchedule.WorkSchedule!= null) 
                SetWorkSchedule(_staffSchedule.WorkSchedule);
        }
    }

    private TypeEmployment _typeEmloyment;

    public TypeEmployment TypeEmloyment
    {
        get => _typeEmloyment;
    }

    public Employee(EmployeeName name, Organization organization, Department department, StaffSchedule staffSchedule)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _organization = organization ?? throw new ArgumentNullException(nameof(_organization));
        _department = department ?? throw new ArgumentNullException(nameof(_department));
        _staffSchedule = staffSchedule ?? throw new ArgumentNullException(nameof(staffSchedule));
    }

    public Employee()
    {
    }

    public void SetTypeEmployment(TypeEmployment typeEmployment)
    {
        _typeEmloyment = typeEmployment ?? throw new ArgumentNullException(nameof(typeEmployment));
    }
    
    private WorkSchedule _workSchedule;

    public WorkSchedule WorkSchedule
    {
        get => _workSchedule;
    }

    public void SetWorkSchedule(WorkSchedule workSchedule)
    {
        _workSchedule = workSchedule ?? throw new ArgumentNullException(nameof(workSchedule));
    }
}

