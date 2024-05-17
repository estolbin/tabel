using web_tabel.Domain;

namespace web_tabel.Domain;

/// <summary>
/// График работы сотрудника
/// </summary>
public class WorkSchedule : Entity
{
    public virtual string Name { get; set; }

    /// <summary>
    /// Дата отсчета графика
    /// </summary>
    public virtual DateTime? ReferenceDate { get; set; }

    /// <summary>
    /// Список дней цикла графика
    /// </summary>
    public virtual ICollection<WorkSchedulleHours> HoursByDayNumbers { get; set; }
 


    /// <summary>
    /// Длительность рабочей недели в часах 
    /// </summary>
    public float HoursInWeek { get; set; }

    public float GetHoursByDate(DateTime workDate, TypeOfWorkingTime typeEmployment)
    {
        var dayNumber = (int)workDate.DayOfWeek;
        var workSchedule = HoursByDayNumbers.FirstOrDefault(x => x.DayNumber == dayNumber);
        return workSchedule != null ? workSchedule.Hours : 0f;
    }

    /// <summary>
    /// Заполняет график по умолчанию - пятидневная рабочая неделя.
    /// </summary>
    /// <param name="workSchedule"></param>
    /// <returns></returns>
    public static List<WorkSchedulleHours> GetDefaultList(WorkSchedule workSchedule)
    {
        return new List<WorkSchedulleHours>
        {
            new WorkSchedulleHours() {DayNumber = 1, Hours = 8f, TypeOfWorkingTime = TypeOfWorkingTime.GetWorkType()},
            new WorkSchedulleHours() {DayNumber = 2, Hours = 8f, TypeOfWorkingTime = TypeOfWorkingTime.GetWorkType()},
            new WorkSchedulleHours() {DayNumber = 3, Hours = 8f, TypeOfWorkingTime = TypeOfWorkingTime.GetWorkType()},
            new WorkSchedulleHours() {DayNumber = 4, Hours = 8f, TypeOfWorkingTime = TypeOfWorkingTime.GetWorkType()},
            new WorkSchedulleHours() {DayNumber = 5, Hours = 8f, TypeOfWorkingTime = TypeOfWorkingTime.GetWorkType()},
            new WorkSchedulleHours() { DayNumber = 6, Hours = 0f, TypeOfWorkingTime = TypeOfWorkingTime.GetWeekend()},
            new WorkSchedulleHours() {DayNumber = 7, Hours = 0f, TypeOfWorkingTime = TypeOfWorkingTime.GetWeekend()},
        };
    }
}
