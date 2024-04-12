namespace web_tabel.Domain;

/// <summary>
/// Запись табеля по сотруднику, на дату, в заданном периоде
/// </summary>
public class TimeShift : Entity
{
    private TimeShiftPeriod _timeShiftPeriod;
    private Employee _employee;
    private WorkSchedule _workSchedule;
    private DateTime _workDate;
    private TypeEmployment _typeEmployment;
    private float _hoursPlanned;
    private float _hoursWorked;

    public float HoursPlanned
    {
        get => _hoursPlanned;
    }

    public TimeShiftPeriod TimeShiftPeriod
    {
        get => _timeShiftPeriod;
        set => _timeShiftPeriod = value;
    }

    public Employee Employee
    {
        get => _employee;
        set => _employee = value;
    }

    public WorkSchedule WorkSchedule
    {
        get => _workSchedule;
        set => _workSchedule = value;
    }

    public DateTime WorkDate
    {
        get => _workDate;
        set => _workDate = value;
    }

    public TypeEmployment TypeEmployment
    {
        get => _typeEmployment;
        set => _typeEmployment = value;
    }

    /// <summary>
    /// Конструктор записи табеля по сотруднику
    /// </summary>
    /// <param name="timeShiftPeriod">Период табеля</param>
    /// <param name="employee">Сотрудник</param>
    /// <param name="workSchedule">График работы сотрудника</param>
    /// <param name="workDate">Дата табеля</param>
    /// <param name="typeEmployment">Вид рабочего времени</param>
    /// <exception cref="Exception"></exception>
    public TimeShift(
        TimeShiftPeriod timeShiftPeriod, 
        Employee employee, 
        WorkSchedule workSchedule, 
        DateTime workDate, 
        TypeEmployment typeEmployment)
    {
        if (timeShiftPeriod.IsClosed()) throw new Exception("Закрытый период");
        
        _timeShiftPeriod = timeShiftPeriod ?? throw new ArgumentNullException(nameof(timeShiftPeriod));
        _employee = employee?? throw new ArgumentNullException(nameof(employee));
        _workSchedule = workSchedule?? throw new ArgumentNullException(nameof(workSchedule));
        _workDate = workDate;
        _typeEmployment = typeEmployment?? throw new ArgumentNullException(nameof(typeEmployment));
        CalculatePlannedHours();
    }

    public TimeShift() {}
    public void CalculatePlannedHours()
    {
        _hoursPlanned = _workSchedule.GetHoursByDate(_workDate, _typeEmployment);
    }
    
    public TimeShift(TimeShiftPeriod period, Employee employee, DateTime workDate) : 
        this(period, employee, employee.WorkSchedule, workDate, employee.TypeEmloyment) {}
}