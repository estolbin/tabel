using System.ComponentModel.DataAnnotations;

namespace web_tabel.Domain;

/// <summary>
/// Период, за который заполняем табель
/// </summary>
public class TimeShiftPeriod : Entity
{
    [DataType(DataType.Date)]
    [Display(Name = "Начало периода")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Start { get; set; }
    [DataType(DataType.Date)]
    [Display(Name = "Конец периода")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime End { get; set; }
    public bool Closed { get; set; }
    [Display(Name = "Наименование")]
    public string Name { get; set; }

    public TimeShiftPeriod(string name, DateTime start, DateTime end, bool closed = false)
    {
        if (start > end) throw new ArgumentException("Start must be before end");
        if (start == end) throw new ArgumentException("Start and End cannot be the same");

        Name = name;
        Start = start;
        End = end;
        Closed = closed;
    }

    public TimeShiftPeriod() { }

    /// <summary>
    /// Возвращает коллекцию дней в периоде
    /// </summary>
    /// <param name="withWeekend">Включать дни выходных</param>
    /// <returns>IEnumerable<DateTime></returns>
    public IEnumerable<DateTime> GetAllDaysInPeriod(bool withWeekend = false)
    {
        List<DateTime> result = new();

        for (DateTime date = Start; date <= End; date = date.AddDays(1))
        {
            if (!withWeekend && (date.DayOfWeek.Equals(DayOfWeek.Saturday) || date.DayOfWeek.Equals(DayOfWeek.Sunday)))
                continue;
            result.Add(date);
        }
        return result;
    }

    public IEnumerable<TimeShift> FilllTimeShiftPlan(Employee employee)
    {
        List<TimeShift> shifts = new();
        foreach (var day in GetAllDaysInPeriod(true))
        {
            var ts = new TimeShift(this, employee, day);
            ts.HoursPlanned = ts.GetPlannedHours(day);

            shifts.Add(ts);
        }

        return shifts.AsEnumerable();
    }
}