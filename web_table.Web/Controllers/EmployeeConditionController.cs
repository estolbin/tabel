using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_table.Web.Controllers
{
    public class EmployeeConditionController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private IEnumerable<TypeOfWorkingTime> _typeOfWorkingTime;

        public EmployeeConditionController()
        {
            _unitOfWork = new UnitOfWork();
        }

        private void SetViewBag()
        {
            if (_typeOfWorkingTime == null)
                _typeOfWorkingTime = _unitOfWork.TypeOfWorkingTimeRepository.GetAll();
            ViewBag.TypeOfWorkingTimeList = _typeOfWorkingTime.ToSelectListItem(x => x.Name, x => x.Description,null);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            SetViewBag();
            var list = await _unitOfWork.EmployeeConditionRepository.GetAllAsync();
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> SetTypeOfWorkingTime(string name, string typeWT)
        {
            typeWT = typeWT ?? "";
            var item = await _unitOfWork.EmployeeConditionRepository.SingleOrDefaultAsync(x => x.Name == name);
            var type = await _unitOfWork.TypeOfWorkingTimeRepository.SingleOrDefaultAsync(x => x.Name == typeWT);
            if (type == null) return NotFound();
            item.TypeOfWorkingTime = type;
            await _unitOfWork.EmployeeConditionRepository.UpdateAsync(item);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
