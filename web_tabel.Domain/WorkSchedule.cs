namespace web_tabel.Domain;

/// <summary>
/// График работы сотрудника
/// </summary>
public class WorkSchedule : Entity
{
    private string _name;

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
}