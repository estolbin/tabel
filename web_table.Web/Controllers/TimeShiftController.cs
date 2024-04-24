using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_tabel.Domain;
using web_table.Web.ViewModel;
using System.Collections.Generic;

namespace web_table.Web.Controllers
{
    public class TimeShiftController : Controller
    {
        private readonly ITimeShiftService _service;
        private IEnumerable<Department> _departments;
        private IEnumerable<Organization> _organizations;

        public TimeShiftController(ITimeShiftService service) => _service = service;

        public async Task<IActionResult> Index()
        {
            SetViewBagForSelect();

            var currentTimeShift = await _service.GetCurrentTimeShift();
            var employeeTimeShiftList = await EmployeeTimeShiftDTO.ToListFromTimeShift(currentTimeShift);
            return View(employeeTimeShiftList);
        }

        private async void SetViewBagForSelect()
        {

            if (_departments == null)
                _departments = await _service.GetAllDepartments();
            if (_organizations == null)
                _organizations = await _service.GetAllOrganization();

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
            var result = await _service.GetTimeShiftByEmpLike(searchText);

            if (result == null || result.ToList().Count < 1)
            {
                ViewData["Message"] = "Не найдено в базе";
                TempData["SearchString"] = "";
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
    }
}