using Microsoft.AspNetCore.Mvc;
using web_tabel.Services;

namespace web_table.Web.Controllers
{
    [Controller]
    public class WorkScheduleController : Controller
    {
        private UnitOfWork _unitOfWork;

        public WorkScheduleController() 
        { 
            _unitOfWork = new UnitOfWork();
        }


        public IActionResult Index()
        {
            var workSchedules = _unitOfWork.WorkScheduleRepository.GetAll();
            return View(workSchedules);
        }

        [HttpGet]
        public Task<IActionResult> Edit(Guid id)
        {
            var workSchedule = _unitOfWork.WorkScheduleRepository.SingleOrDefault(x => x.Id == id);
            return Task.FromResult<IActionResult>(View(workSchedule));
        }
    }
}
