namespace web_tabel.Domain;

/// <summary>
/// Запись табеля по сотруднику, на дату, в заданном периоде
/// </summary>
public class TimeShift : Entity
{
    private static TypeOfWorkingTime _workType = TypeOfWorkingTime.GetWorkType();
    private static TypeOfWorkingTime _weekendType = TypeOfWorkingTime.GetWeekend();

    public float HoursPlanned { get; set; }
    public float HoursWorked { get; set; }

    public virtual TimeShiftPeriod TimeShiftPeriod { get; set; }

    public virtual Employee Employee { get; set; }

    /// <summary>
    /// График работы сотрудника
    /// </summary>
    public virtual WorkSchedule WorkSchedule { get; set; }

    public DateTime WorkDate { get; set; }

    /// <summary>
    /// Вид рабочего времени
    /// </summary>
    public virtual TypeOfWorkingTime? TypeEmployment { get; set; }

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
        TypeOfWorkingTime typeEmployment)
    {
        if (timeShiftPeriod.Closed) throw new Exception("Закрытый период");

        TimeShiftPeriod = timeShiftPeriod ?? throw new ArgumentNullException(nameof(timeShiftPeriod));
        Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        WorkSchedule = workSchedule ?? throw new ArgumentNullException(nameof(workSchedule));
        WorkDate = workDate;
        CheckTypeEmloyment();
    }

    private void CheckTypeEmloyment()
    {
        HoursPlanned = GetPlannedHours(WorkDate);
        if (HoursPlanned > 0)
        {
            if (TypeEmployment == null) TypeEmployment = _workType;
        }
        else
        {
            if (TypeEmployment != TypeOfWorkingTime.GetWeekend()) TypeEmployment = _weekendType;
        }
    }

    public TimeShift() { }

    public float GetPlannedHours(DateTime day)
    {
        return WorkSchedule.GetHoursByDate(WorkDate, TypeEmployment);
    }

    public TimeShift(TimeShiftPeriod period, Employee employee, DateTime workDate) :
        this(period, employee, employee.WorkSchedule, workDate, null)
    { }
}