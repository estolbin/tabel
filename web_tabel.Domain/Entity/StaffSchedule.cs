namespace web_tabel.Domain;

/// <summary>
/// Штатное расписание
/// </summary>
public class StaffSchedule : Entity
{
    // aggregate root
    public virtual Organization Organization { get; set; }

    public virtual Department Department { get; set; }

    public Guid? ParentId { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Количество единиц
    /// </summary>
    public float NumberOfPositions { get; set; }

    public virtual WorkSchedule WorkSchedule { get; set; }

    public StaffSchedule(Organization organization, Department department, WorkSchedule workSchedule, string name)
    {
        Organization = organization ?? throw new ArgumentNullException(nameof(organization));
        Department = department ?? throw new ArgumentNullException(nameof(department));
        WorkSchedule = workSchedule ?? throw new ArgumentNullException(nameof(workSchedule));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public StaffSchedule() { }


}