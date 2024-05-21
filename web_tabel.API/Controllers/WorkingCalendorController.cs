using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_tabel.API.Controllers
{
    [Route("api/WorkCalendar")]
    [ApiController]
    public class WorkingCalendorController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public WorkingCalendorController()
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpPost("AddCalendar")]
        public async Task<IActionResult> AddWorkCalendar([FromBody] WorkingCalendar workCalendar)
        {
            var exist = await _unitOfWork.WorkingCalendarRepository.SingleOrDefaultAsync(x => x.Date == workCalendar.Date);
            if (exist != null)
            {
                return BadRequest("WorkCalendar already exists");
            }
            workCalendar.Year = workCalendar.Date.Year;
            await _unitOfWork.WorkingCalendarRepository.InsertAsync(workCalendar);
            await _unitOfWork.SaveAsync();

            return Ok();
        }

        [HttpGet("GetAllCalendar")]
        public async Task<IActionResult> GetAllWorkCalendar()
        {
            return Ok(await _unitOfWork.WorkingCalendarRepository.GetAllAsync());
        }

    }
}
