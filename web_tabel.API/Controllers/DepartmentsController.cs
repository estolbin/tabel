using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_tabel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public DepartmentsController()
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpGet("GetAllDepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            return Ok( await _unitOfWork.DepartmentRepository.GetAllAsync());
        }

        [HttpPost("AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromBody] Department department)
        {
            if (await _unitOfWork.DepartmentRepository.SingleOrDefaultAsync(x => x.Id == department.Id) != null) { return BadRequest("Department already exists"); }
            var organizaton = await _unitOfWork.OrganizationRepository.SingleOrDefaultAsync(x => x.Id == department.Organization.Id);
            if (organizaton != null)
            {
                department.Organization = organizaton;
                await _unitOfWork.DepartmentRepository.InsertAsync(department);
                await _unitOfWork.SaveAsync();
            }
            return Ok("Success");
        }

        [HttpPut("UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment([FromBody] Department department)
        {
            await _unitOfWork.DepartmentRepository.InsertAsync(department);
            await _unitOfWork.SaveAsync();
            return Ok("Success");
        }

        [HttpGet("GetDepartmentById")]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            return Ok(await _unitOfWork.DepartmentRepository.SingleOrDefaultAsync(x => x.Id == id));
        }
    }
}
