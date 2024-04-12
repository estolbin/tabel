namespace web_tabel.Domain;

public interface ITimeShiftRepository
{
    public Task<IEnumerable<TimeShift>> GetTimeShiftsByPeriod(TimeShiftPeriod period);
    public Task<TimeShiftPeriod> GetTimeShiftsPeriodByDate(DateTime date);
    public Task<IEnumerable<TimeShiftPeriod>> GetAllPeriods();
    
    public Task<TimeShiftPeriod> GetLastPeriod();
    
}