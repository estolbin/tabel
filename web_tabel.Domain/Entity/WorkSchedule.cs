namespace web_tabel.Domain;

/// <summary>
/// График работы сотрудника
/// </summary>
public class WorkSchedule : Entity
{
    public virtual string Name { get; set; }
    /// <summary>
    /// Словарь, где ключ - номер дня недели, значение - часы работы
    /// </summary>
    public Dictionary<int, float> HoursByDayNumbers = DefaultHoursOfWork();

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



    /// <summary>
    /// Длительность рабочей недели в часах 
    /// </summary>
    public float HoursOfWork { get; set; }

    public float GetHoursByDate(DateTime workDate, TypeOfWorkingTime typeEmployment)
    {
        return HoursByDayNumbers.ContainsKey((int)workDate.DayOfWeek) ? HoursByDayNumbers[(int)workDate.DayOfWeek] : 0f;
    }
}