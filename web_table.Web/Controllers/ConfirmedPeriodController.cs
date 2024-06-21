using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_table.Web.Controllers
{
    public class ConfirmedPeriodController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ITimeShiftService _service;

        public ConfirmedPeriodController(ITimeShiftService service)
        {
            _unitOfWork = new UnitOfWork();
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var deps = await _service.GetAllDepartments();
            var list = await _unitOfWork.ConfirmedPeriodRepository.GetAsync(x => deps.Contains(x.Department));
            return View(list.OrderByDescending(x => x.Period.Start));
        }

        [HttpPost]
        public IActionResult SaveConfirm(string periodId, string departmentId, bool HalfValue, string WhichHalf)
        {
            var confirm = _unitOfWork.ConfirmedPeriodRepository.SingleOrDefault(x => x.PeriodId == Guid.Parse(periodId) && x.DepartmentId == Guid.Parse(departmentId));
            if (WhichHalf == "First")
                confirm.FirstHalfIsConfirmed = !HalfValue;
            else 
                confirm.SecondHalfIsConfirmed = !HalfValue;

            _unitOfWork.ConfirmedPeriodRepository.Update(confirm);
            _unitOfWork.Save();
            return Json( new { redirectToUrl = Url.Action("Index", "ConfirmedPeriod") } );
        }
    }
}
