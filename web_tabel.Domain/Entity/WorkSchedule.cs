namespace web_tabel.Domain;

/// <summary>
/// График работы сотрудника
/// </summary>
public class WorkSchedule : Entity
{
    private string _name;
    /// <summary>
    /// Словарь, где ключ - номер дня недели, значение - часы работы
    /// </summary>
    private Dictionary<int, float> _hoursByDayNumbers = DefaultHoursOfWork();

    /// <summary>
    /// Стандартный график для пятидневного графика
    /// </summary>
    /// <returns></returns>
    private static Dictionary<int, float> DefaultHoursOfWork()
    {
        return new Dictionary<int, float>()
        {
            { 1, 8f }, //пн
            { 2, 8f }, //вт
            { 3, 8f }, //ср
            { 4, 8f }, //чт
            { 5, 8f }, //пт
            { 6, 0f }, //сб
            { 7, 0f } //вс
        };
    }

    public void SetHoursOfWork(Dictionary<int, float> hoursOfWork)
    {
        _hoursByDayNumbers = hoursOfWork;
    }

    public string Name
    {
        get => _name;
        set
        {
            if (value.Length > 100)
                throw new ArgumentException("Название графика не может быть больше 100 символов");
            _name = value;
        }
    }

    private float _hoursOfWork;
    /// <summary>
    /// Длительность рабочей недели в часах 
    /// </summary>
    public float HoursOfWork
    {
        get => _hoursOfWork;
        set => _hoursOfWork = value;
    }

    public float GetHoursByDate(DateTime workDate, TypeOfWorkingTime typeEmployment)
    {
        return _hoursByDayNumbers.ContainsKey((int)workDate.DayOfWeek) ? _hoursByDayNumbers[(int)workDate.DayOfWeek] : 0f;
    }
}