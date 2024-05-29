using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_tabel.API.Controllers
{
    [Route("api/Condition")]
    [ApiController]
    public class EmployeeConditionController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public EmployeeConditionController() 
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpGet("GetAllCondition")]
        public async Task<IActionResult> GetAllCondition()
        {
            return Ok(await _unitOfWork.EmployeeConditionRepository.GetAllAsync());
        }

        [HttpPost("AddCondition")]
        public async Task<IActionResult> AddCondition([FromBody] EmployeeCondition condition)
        {
            var exist = await _unitOfWork.EmployeeConditionRepository.SingleOrDefaultAsync(x => x.Name == condition.Name);
            if (exist != null) { return BadRequest("Condition already exists"); }

            _unitOfWork.EmployeeConditionRepository.Insert(condition);
            _unitOfWork.Save();

            return Ok();
        }


    }
}
