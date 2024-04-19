using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_table.Web.ViewModel;

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

            var departments = _service.GetAllDepartments().Result;
            ViewBag.Departments = departments;

            var result = _service.GetCurrentTimeShift().Result;
            var r = EmployeeTimeShiftDTO.ToListFromTimeShift(result);
            return View(r);
        }

        [HttpPost]
        public IActionResult Index(string? departmentId)
        {
            if (departmentId == null) { return RedirectToAction("Index"); }
            ViewBag.Departments = _service.GetAllDepartments().Result;
            
            var result = _service.GetTimeShiftsByDepartment(new Guid(departmentId)).Result;

            var r = EmployeeTimeShiftDTO.ToListFromTimeShift(result);
            return View("Index", r);
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
