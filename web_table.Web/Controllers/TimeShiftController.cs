using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_tabel.Domain;
using web_table.Web.ViewModel;

namespace web_table.Web.Controllers
{
    public class TimeShiftController : Controller
    {
        private readonly ITimeShiftService _service;

        public TimeShiftController(ITimeShiftService service) => _service = service;

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
            
            IEnumerable<SelectListItem> list = listDeps.ToSelectListItem(x => x.Id, x => string.Format("{0} ({1})", x.Name, x.Organization.Name), TempData["depId"] as string[]);
            ViewBag.Departments = list;

            IEnumerable<Organization> listOrgs = _service.GetAllOrganization().Result;
            ViewBag.OrganizationsRaw = listOrgs;
            IEnumerable<SelectListItem> listOrg = listOrgs.ToSelectListItem(x => x.Id, x => x.Name, TempData["orgId"] as string[]);
            ViewBag.Organizations = listOrg;
        }

        [HttpPost]
        public IActionResult Index(string[] depId, string[] orgId, bool isDepartment = false, bool isOrganization = false) 
        {
            IEnumerable<TimeShift> result = new List<TimeShift>();
            List<Guid> ids = new List<Guid>();

            if (isDepartment)
            {
                if (depId.Length == 0) { return RedirectToAction("Index"); }
                foreach(var item in depId)
                {
                    ids.Add(new Guid(item));
                }
                result = _service.GetTimeShiftByDepartments(ids).Result;
                SetTempData("depId", depId);
            } else if (isOrganization )
            {
                if(orgId.Length == 0) { return RedirectToAction("Index"); }
                foreach(var item in orgId)
                {
                    ids.Add(new Guid(item));
                }
                result = _service.GetTimeShiftByOrganizations(ids).Result;
                SetTempData("orgId", orgId);
            }
            SetViewBagForSelect();
            var r = EmployeeTimeShiftDTO.ToListFromTimeShift(result);
            return View("Index",r);
        }

        private void SetTempData(string keyName, string[] value) =>  TempData[keyName] = value;

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

        public async Task<IActionResult> SetNewHours(Guid empid, DateTime curDate) => View(_service.GetTimeShiftByEmpAndDate(empid, curDate).Result);

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
