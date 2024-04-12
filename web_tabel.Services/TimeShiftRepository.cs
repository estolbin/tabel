using Microsoft.EntityFrameworkCore;
using web_tabel.Domain;
using web_tabel.Services.TimeShiftContext;

namespace web_tabel.Services;

public class TimeShiftRepository : ITimeShiftRepository
{
    private readonly TimeShiftDBContext _context;

    public TimeShiftRepository(TimeShiftDBContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<TimeShift>> GetTimeShiftsByPeriod(TimeShiftPeriod period)
    {
        return await _context.TimeShifts.Where(x => x.TimeShiftPeriod == period).ToListAsync();
    }

    public async Task<TimeShiftPeriod> GetTimeShiftsPeriodByDate(DateTime date)
    {
        var found = _context.TimeShiftPeriods.FirstOrDefaultAsync(x => x.Start <= date && x.End >= date);
        if (found is null)
            throw new KeyNotFoundException();
        return found.Result;
    }

    public async Task<IEnumerable<TimeShiftPeriod>> GetAllPeriods()
    {
        return await _context.TimeShiftPeriods.ToListAsync();
    }

    public async Task<TimeShiftPeriod> GetLastPeriod()
    {
        var found = _context.TimeShiftPeriods.LastOrDefaultAsync();
        return found.Result;
    }
}