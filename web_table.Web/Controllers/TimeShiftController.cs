using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using web_tabel.Domain;
using web_tabel.Services;
using web_table.Web.ViewModel;

namespace web_table.Web.Controllers
{
    public class TimeShiftController : Controller
    {
        private readonly ITimeShiftService _service;
        private readonly UnitOfWork _unitOfWork;
        private IEnumerable<Department> _departments;
        private IEnumerable<Organization> _organizations;
        private IEnumerable<TimeShiftPeriod> _periods;
        private IEnumerable<TypeOfWorkingTime> _typeOfWorkingTimes;

        public TimeShiftController(ITimeShiftService service)
        {
            _service = service;
            _unitOfWork = new UnitOfWork();
        }

        private async Task<Guid> GetGuidFromSession()
        {
            var periodId = HttpContext.Session.GetString("SessionPeriodId");
            // if periodId == null, we must take last period
            var dbPeriod = await _service.GetLastPeriod();
            if (periodId == null)
            {
                periodId = dbPeriod == null ? null : dbPeriod.Id.ToString();
            };
            return periodId == null ? Guid.Empty : new Guid(periodId);
        }

        public async Task<IActionResult> Index()
        {
            SetViewBagForSelect();

            var periods = await _service.GetAllPeriods();
            string periodsJson = JsonConvert.SerializeObject(periods);
            HttpContext.Session.SetString("SessionPeriods", periodsJson);

            _typeOfWorkingTimes = await _service.GetAllTypeOfWorkingTime();

            var periodId = GetGuidFromSession().Result;
            IEnumerable<TimeShift> currentTimeShift = await _service.GetCurrentTimeShift(periodId);
            if (currentTimeShift == null || !currentTimeShift.Any()) return View("Clean");
            var employeeTimeShiftList = await EmployeeTimeShiftViewModel.ToListFromTimeShift(currentTimeShift);
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
            else TempData["ErrorMessage"] = "";

            var employeeTimeShiftList = await EmployeeTimeShiftViewModel.ToListFromTimeShift(result);
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
            var result = await _service.GetTimeShiftByEmpLike(searchText);

            if (result == null || !result.Any())
            {
                TempData["SearchString"] = "";
                TempData["ErrorMessage"] = "По вашему запросу ничего не найдено";
                return RedirectToAction("Index");
            }

            var employeeTimeShiftList = await EmployeeTimeShiftViewModel.ToListFromTimeShift(result);
            TempData["SearchString"] = searchText;
            return View("Index", employeeTimeShiftList);
        }

        private DateTime EmptyDate = new DateTime(0001, 01, 01);

        public async Task<IActionResult> SetNewHours(Guid empId, DateTime currentDate)
        {

            var periodId = await GetGuidFromSession();
            var timeShifts = await _service.GetTimeShiftByEmpAndDate(empId, currentDate, periodId);

            if (_typeOfWorkingTimes == null) _typeOfWorkingTimes = await _service.GetAllTypeOfWorkingTime();

            IEnumerable<TypeOfWorkingTime> list = new List<TypeOfWorkingTime>();
            if (timeShifts.TypeEmploymentPlanned != null)
            {
                var listRules = await _unitOfWork.TypeOfWorkingTimeRulesRepository.GetAsync(x => x.Source == timeShifts.TypeEmploymentPlanned);
                if (listRules.Count() > 0)
                    list = listRules.Select(x => x.Target);
                else
                    list = _typeOfWorkingTimes;
            }
            else
            {
                list = _typeOfWorkingTimes;
            }
            ViewBag.Types = list.ToSelectListItem(x => x.Name, x => x.Description, new[] { timeShifts.TypeEmploymentWorked?.Name ?? String.Empty});

            return View(timeShifts);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNewHours(TimeShift timeShift, string typeWTName)
        {
            var existingTimeShift = await _service.GetTimeShiftByID(timeShift.Id);
            existingTimeShift.HoursWorked = timeShift.HoursWorked;
            existingTimeShift.TypeEmploymentWorked = await _service.GetTypeOfWorkingTime(typeWTName);
            await _service.UpdateTimeShift(existingTimeShift);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SavePeriodId(String  periodId)
        {
            HttpContext.Session.SetString("SessionPeriodId", periodId);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> FillInDay(DateTime date)
        {
            TempData["DateForFill"] = date;
            return View(date);
        }

        [HttpPost]
        public async Task<IActionResult> FillInDate()
        {
            DateTime fillData = (DateTime)TempData["DateForFill"];

            if (fillData == EmptyDate) return Json(new { redirectToUrl = Url.Action("Index", "TimeShift") });

            var listTimeShift = await _unitOfWork.TimeShiftRepository.GetAsync(x => x.WorkDate == fillData);
            for (int i = 0; i < listTimeShift.Count(); i++)
            {
                var timeShift = listTimeShift.ElementAt(i);

                if (timeShift.HoursWorked == 0 && timeShift.TypeEmploymentWorked == null)
                {
                    timeShift.HoursWorked = timeShift.HoursPlanned;
                    timeShift.TypeEmploymentWorked = timeShift.TypeEmploymentPlanned;
                }
                else
                    continue;

                await _unitOfWork.TimeShiftRepository.UpdateAsync(timeShift);
                await _unitOfWork.SaveAsync();
            }

            return Json(new { redirectToUrl = Url.Action("Index", "TimeShift") });
        }

    }
}