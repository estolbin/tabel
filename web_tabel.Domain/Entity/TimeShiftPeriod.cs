namespace web_tabel.Domain;

public class TimeShiftPeriod : Entity
{
    private DateTime _start;
    private DateTime _end;
    private bool _closed;
    private string _name;
    
    public DateTime Start => _start;
    public DateTime End => _end;
    public bool Closed => _closed;
    
    public string Name => _name;

    public TimeShiftPeriod(string name, DateTime start, DateTime end, bool closed = false)
    {
        if (start > end) throw new ArgumentException("Start must be before end");
        if (start == end) throw new ArgumentException("Start and End cannot be the same");
        
        _name = name;
        _start = start;
        _end = end;
        _closed = closed;
    }
    
    public TimeShiftPeriod() {}

    public bool IsClosed()
    {
        return _closed;
    }

    /// <summary>
    /// Возвращает коллекцию дней в периоде
    /// </summary>
    /// <param name="withWeekend">Включать дни выходных</param>
    /// <returns>IEnumerable<DateTime></returns>
    public IEnumerable<DateTime> GetAllDaysInPeriod(bool withWeekend = false)
    {
        List<DateTime> result = new ();
        
        for (DateTime date = _start; date <= _end; date = date.AddDays(1))
        {
            if (!withWeekend && (date.DayOfWeek.Equals(DayOfWeek.Saturday) || date.DayOfWeek.Equals(DayOfWeek.Sunday)))
                continue;
            result.Add(date);
        }
        return result;
    }

    public void FilllTimeShiftPlan(Employee employee)
    {
        List<TimeShift> shifts = new ();
        foreach (var day in GetAllDaysInPeriod())
        {
            shifts.Add(new TimeShift(this, employee, day));
        }
    }
}