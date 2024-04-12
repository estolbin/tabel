namespace web_tabel.Domain;

public interface ITimeShiftService
{
    public Task<IEnumerable<TimeShift>> GetTimeShifts(TimeShiftPeriod period, CancellationToken token = default);
    public Task<TimeShiftPeriod> GetLastUnclosedPeriod(CancellationToken token = default);
    public Task<TimeShiftPeriod> GetPeriodByDate(DateTime date, CancellationToken token = default);
    public Task<TimeShiftPeriod> GetLastPeriod(CancellationToken token = default);
}