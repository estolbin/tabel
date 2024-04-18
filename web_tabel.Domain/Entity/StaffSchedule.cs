namespace web_tabel.Domain;

/// <summary>
/// Штатное расписание
/// </summary>
public class StaffSchedule : Entity
{
    // aggregate root
    public Organization Organization {  get; set; }

    public Department Department {  get; set; }

    public Position Position {  get; set; }

    public Guid? ParentId { get; set; }
    
    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Количество единиц
    /// </summary>
    public float NumberOfPositions {  get; set; }

     public WorkSchedule WorkSchedule { get; set; }

    public StaffSchedule(Organization organization, Department department, Position position, WorkSchedule workSchedule, string name)
    {
        Organization = organization ?? throw new ArgumentNullException(nameof(organization));
        Department = department ?? throw new ArgumentNullException(nameof(department));
        Position = position ?? throw new ArgumentNullException(nameof(position));
        WorkSchedule = workSchedule?? throw new ArgumentNullException(nameof(workSchedule));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
    
    public StaffSchedule() {}


}