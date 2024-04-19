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

            ViewBag.Departments = _service.GetAllDepartments().Result; 
            ViewBag.Organizations = _service.GetAllOrganization().Result;

            var result = _service.GetCurrentTimeShift().Result;
            var r = EmployeeTimeShiftDTO.ToListFromTimeShift(result);
            return View(r);
        }

        [HttpPost]
        public IActionResult Index(string? depId, string? orgId, bool isDepartment = false, bool isOrganization = false) 
        {
            ViewBag.Departments = _service.GetAllDepartments().Result;
            ViewBag.Organizations = _service.GetAllOrganization().Result;

            IEnumerable<TimeShift> result = new List<TimeShift>();
            if (isDepartment)
            {
                if (depId == null) { return RedirectToAction("Index"); }
                result = _service.GetTimeShiftsByDepartment(new Guid(depId)).Result;
            } else if (isOrganization )
            {
                if(orgId == null) { return RedirectToAction("Index"); }
                result = _service.GetTimeShiftByOrganization(new Guid(orgId)).Result;
                
            }

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
