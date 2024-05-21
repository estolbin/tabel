using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_tabel.API.Controllers
{
    [Route("api/TypeEmployment")]
    [ApiController]
    public class TypeEmploymentController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public TypeEmploymentController() 
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpGet("GetAllTypeEmployment")]
        public Task<IActionResult> GetAllTypeEmployment()
        {
            var result = _unitOfWork.TypeOfEmploymentRepository.GetAll();
            return Task.FromResult<IActionResult>(Ok(result));
        }

        [HttpPost("AddTypeEmployment")]
        public async Task<IActionResult> AddTypeEmployment([FromBody] TypeOfEmployment typeEmployment)
        {
            if (_unitOfWork.TypeOfEmploymentRepository.SingleOrDefault(x => x.Name == typeEmployment.Name) != null) return BadRequest("TypeEmployment already exists");

            typeEmployment.Name = typeEmployment.Name.Replace("_", " ");

            await _unitOfWork.TypeOfEmploymentRepository.InsertAsync(typeEmployment);
            await _unitOfWork.SaveAsync();

            return Ok();
        }
    }
}
