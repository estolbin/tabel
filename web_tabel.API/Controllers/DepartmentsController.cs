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
        public Task<IActionResult> GetAllDepartments()
        {
            return Task.FromResult<IActionResult>(Ok(_unitOfWork.DepartmentRepository.GetAll()));
        }

        [HttpPost("AddDepartment")]
        public Task<IActionResult> AddDepartment([FromBody] Department department)
        {
            if (_unitOfWork.DepartmentRepository.SingleOrDefault(x => x.Id == department.Id) != null) { return Task.FromResult<IActionResult>(BadRequest("Department already exists")); }
            var organizaton = _unitOfWork.OrganizationRepository.SingleOrDefault(x => x.Id == department.Organization.Id);
            if (organizaton != null)
            {
                department.Organization = organizaton;
                _unitOfWork.DepartmentRepository.Insert(department);
                _unitOfWork.Save();
            }
            return Task.FromResult<IActionResult>(Ok("Success"));
        }

        [HttpPut("UpdateDepartment")]
        public Task<IActionResult> UpdateDepartment([FromBody] Department department)
        {
            _unitOfWork.DepartmentRepository.Update(department);
            _unitOfWork.Save();
            return Task.FromResult<IActionResult>(Ok("Success"));
        }

        [HttpGet("GetDepartmentById")]
        public Task<IActionResult> GetDepartmentById(Guid id)
        {
            return Task.FromResult<IActionResult>(Ok(_unitOfWork.DepartmentRepository.SingleOrDefault(x => x.Id == id)));
        }
    }
}
