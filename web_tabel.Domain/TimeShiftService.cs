namespace web_tabel.Domain;

public class TimeShiftService : ITimeShiftService
{
    private readonly ITimeShiftRepository _repository;

    public TimeShiftService(ITimeShiftRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }
    
    public async Task<IEnumerable<TimeShift>> GetTimeShifts(TimeShiftPeriod period, CancellationToken token = default)
    {
        var result = await _repository.GetTimeShiftsByPeriod(period);
        return result;
    }

    public async Task<TimeShiftPeriod> GetLastUnclosedPeriod(CancellationToken token = default)
    {
        var period = await _repository.GetAllPeriods();
        var result = period.LastOrDefault(x => x.IsClosed() == false);
        return result;
    }

    public async Task<TimeShiftPeriod> GetPeriodByDate(DateTime date, CancellationToken token = default)
    {
        var period = await _repository.GetTimeShiftsPeriodByDate(date);
        return period;
    }

    public async Task<TimeShiftPeriod> GetLastPeriod(CancellationToken token = default)
    {
        var period = await _repository.GetLastPeriod();
        return period;
    }
}