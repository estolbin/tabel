using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;

namespace web_table.Web.Controllers
{
    public class TimeShiftController : Controller
    {
        private readonly ITimeShiftService _service;

        public TimeShiftController(ITimeShiftService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var result = _service.GetCurrentTimeShift();
            return View(result.Result);
        }

        public async Task<IActionResult> SetNewHours(Guid empid, DateTime curDate)
        {
            return View(_service.GetTimeShiftByEmpAndDate(empid, curDate).Result);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateNewHours(TimeShift timeShift)
        {
            var ts = await _service.GetTimeShiftByID(timeShift.Id);
            ts.HoursWorked = timeShift.HoursWorked;
            _service.UpdateTimeShift(ts);

            return RedirectToAction(nameof(Index));
        }
    }
}
