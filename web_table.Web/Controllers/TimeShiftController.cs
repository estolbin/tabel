using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using web_tabel.Domain;
using web_table.Web.ViewModel;

namespace web_table.Web.Controllers
{
    public class TimeShiftController : Controller
    {
        private readonly ITimeShiftService _service;
        private IEnumerable<Department> _departments;
        private IEnumerable<Organization> _organizations;
        private IEnumerable<TimeShiftPeriod> _periods;

        public TimeShiftController(ITimeShiftService service) => _service = service;

        public async Task<IActionResult> Index()
        {
            SetViewBagForSelect();

            var periods = await _service.GetAllPeriods();
            string periodsJson = JsonConvert.SerializeObject(periods);
            HttpContext.Session.SetString("SessionPeriods", periodsJson);


            IEnumerable<TimeShift> currentTimeShift = await _service.GetCurrentTimeShift();
            if (currentTimeShift == null || !currentTimeShift.Any()) return View("Clean");
            var employeeTimeShiftList = EmployeeTimeShiftDTO.ToListFromTimeShift(currentTimeShift).Result;
            return View(employeeTimeShiftList);
        }

        private async void SetViewBagForSelect()
        {

            if (_departments == null)
                _departments = await _service.GetAllDepartments();
            if (_organizations == null)
                _organizations = await _service.GetAllOrganization();
            if (_periods == null)
                _periods = await _service.GetAllPeriods();

            ViewBag.DepartmentsRaw = _departments;
            ViewBag.Departments = _departments.ToSelectListItem(x => x.Id, x => $"{x.Name} ({x.Organization.Name})", TempData["depId"] as string[]);

            ViewBag.OrganizationsRaw = _organizations;
            ViewBag.Organizations = _organizations.ToSelectListItem(x => x.Id, x => x.Name, TempData["orgId"] as string[]);

        }

        [HttpPost]
        public async Task<IActionResult> Index(string[] depId, string[] orgId, bool isDepartment = false, bool isOrganization = false)
        {
            IEnumerable<TimeShift> result = new List<TimeShift>();
            List<Guid> ids = new List<Guid>();

            if (isDepartment)
            {
                if (depId.Length == 0)
                    return RedirectToAction("Index");

                foreach (var item in depId)
                {
                    ids.Add(new Guid(item));
                }
                result = await _service.GetTimeShiftByDepartments(ids);
                SetTempData("depId", depId);
            }
            else if (isOrganization)
            {
                if (orgId.Length == 0)
                    return RedirectToAction("Index");

                foreach (var item in orgId)
                {
                    ids.Add(new Guid(item));
                }
                result = await _service.GetTimeShiftByOrganizations(ids);
                SetTempData("orgId", orgId);
            }
            SetViewBagForSelect();

            TempData["ErrorMessage"] = "По вашему запросу ничего не найдено";
            if (!result.Any()) return RedirectToAction("Index");

            var employeeTimeShiftList = await EmployeeTimeShiftDTO.ToListFromTimeShift(result);
            return View("Index", employeeTimeShiftList);
        }

        private void SetTempData(string keyName, string[] value) => TempData[keyName] = value;

        [HttpPost]
        public async Task<IActionResult> Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                TempData["SearchString"] = "";
                return RedirectToAction("Index");
            }

            SetViewBagForSelect();
            var result = _service.GetTimeShiftByEmpLike(searchText).Result;

            if (result == null || !result.Any())
            {
                TempData["SearchString"] = "";
                TempData["ErrorMessage"] = "По вашему запросу ничего не найдено";
                return RedirectToAction("Index");
            }

            var employeeTimeShiftList = await EmployeeTimeShiftDTO.ToListFromTimeShift(result);
            TempData["SearchString"] = searchText;
            return View("Index", employeeTimeShiftList);
        }

        public async Task<IActionResult> SetNewHours(Guid empId, DateTime curDate) => View(_service.GetTimeShiftByEmpAndDate(empId, curDate).Result);

        [HttpPost]
        public async Task<IActionResult> UpdateNewHours(TimeShift timeShift)
        {
            var existingTimeShift = await _service.GetTimeShiftByID(timeShift.Id);
            existingTimeShift.HoursWorked = timeShift.HoursWorked;
            _service.UpdateTimeShift(existingTimeShift);

            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> ShowError(string message)
        //{
        //    return View("Error", message);
        //}
    }
}