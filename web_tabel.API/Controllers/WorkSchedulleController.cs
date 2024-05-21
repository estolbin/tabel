using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_tabel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkSchedulleController : ControllerBase
    {

        private UnitOfWork _unitOfWork;
        public WorkSchedulleController()
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpGet("GetAllWorkSchedulle")]
        public Task<IActionResult> GetAllWorkSchedulle()
        {
            return Task.FromResult<IActionResult>(Ok(_unitOfWork.WorkScheduleRepository.GetAll()));
        }

        [HttpPost("AddWorkSchedulle")]
        public Task<IActionResult> AddWorkSchedulle([FromBody] WorkSchedule workSchedule)
        {
            var schedulle = _unitOfWork.WorkScheduleRepository.SingleOrDefault(x => x.Id == workSchedule.Id);
            if ( schedulle != null)
            {
                return Task.FromResult<IActionResult>(BadRequest("WorkSchedulle already exists"));
            }

            foreach (var day in workSchedule.HoursByDayNumbers)
            {
                
                day.TypeOfWorkingTime = _unitOfWork.TypeOfWorkingTimeRepository.SingleOrDefault(x => x.Name == day.TypeOfWorkingTime.Name) ?? day.TypeOfWorkingTime;

            }

            _unitOfWork.WorkScheduleRepository.Insert(workSchedule);
            _unitOfWork.Save();

            return Task.FromResult<IActionResult>(Ok("Success"));
        }
    }
}
