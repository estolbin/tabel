using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_tabel.Domain;
using web_table.Web.ViewModel;

namespace web_table.Web.Controllers
{
    public class TimeShiftController : Controller
    {
        private readonly ITimeShiftService _service;
        private string SearchText;

        public TimeShiftController(ITimeShiftService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            SetViewBagForSelect();

            var result = _service.GetCurrentTimeShift().Result;
            var r = EmployeeTimeShiftDTO.ToListFromTimeShift(result);
            return View(r);
        }

        private void SetViewBagForSelect()
        {
            IEnumerable<Department> listDeps = _service.GetAllDepartments().Result;
            ViewBag.DepartmentsRaw  = listDeps; // для вывода по подразделениям
            IEnumerable<SelectListItem> list = listDeps.AsEnumerable().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = string.Format("{0} ({1})", x.Name, x.Organization.Name)
            });
            ViewBag.Departments = list;
            ViewBag.Organizations = _service.GetAllOrganization().Result;
        }

        [HttpPost]
        public IActionResult Index(string? depId, string? orgId, bool isDepartment = false, bool isOrganization = false) 
        {
            SetViewBagForSelect();

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

        [HttpPost]
        public IActionResult Search(string? searchText)
        {
            if (searchText == null) 
            {
                TempData["SearchString"] = "";
                return RedirectToAction("Index"); 
            }
            SetViewBagForSelect();

            var result = _service.GetTimeShiftByEmpLike(searchText).Result;

            if (result == null || result.ToList().Count < 1) 
            {
                ViewData["Message"] = "Не найдено в базе";
                TempData["SearchString"] = "";
                return RedirectToAction("Index"); 
            }

            var r = EmployeeTimeShiftDTO.ToListFromTimeShift(result);


            TempData["SearchString"] = searchText;
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
