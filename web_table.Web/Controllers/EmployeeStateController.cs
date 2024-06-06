using Microsoft.AspNetCore.Mvc;
using web_tabel.Services;

namespace web_table.Web.Controllers
{
    public class EmployeeStateController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public EmployeeStateController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public async Task<IActionResult> Index()
        {
            var list = await _unitOfWork.EmployeeStateRepository.GetAllAsync();
            return View(list.OrderBy(x => x.StartDate));
        }
    }
}
