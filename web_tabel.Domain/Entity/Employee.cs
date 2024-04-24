namespace web_tabel.Domain;

/// <summary>
/// Сотрудник
/// </summary>
public class Employee : Entity
{
    /// <summary>
    /// Полные фаилия, имя, отчество. Без сокращений.
    /// </summary>
    public virtual EmployeeName Name { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual Department Department { get; set; }


    /// <summary>
    /// Должность по штатному расписанию.
    /// </summary>
    public virtual StaffSchedule StaffSchedule { get; set; }

    /// <summary>
    /// Вид рабочего времени
    /// </summary>
    public virtual TypeOfWorkingTime TypeEmployment { get; set; }

    /// <summary>
    /// Вид занятости (основное место работы, совместительство)
    /// </summary>
    public virtual TypeOfEmployment TypeOfEmployment { get; set; }

    public Employee(EmployeeName name, Organization organization, Department department, StaffSchedule staffSchedule)
    {
        Name = name ?? throw new ArgumentNullException(nameof(Name));
        Organization = organization ?? throw new ArgumentNullException(nameof(Organization));
        Department = department ?? throw new ArgumentNullException(nameof(Department));
        StaffSchedule = staffSchedule ?? throw new ArgumentNullException(nameof(StaffSchedule));
    }

    public Employee()
    {
    }


    public virtual WorkSchedule WorkSchedule { get; set; }
}

