using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_tabel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public EmployeeController() 
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            return Ok(await _unitOfWork.EmployeeRepository.GetAllAsync());
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {

            employee.Name.Id = employee.Id;
            var existing = await _unitOfWork.EmployeeRepository.SingleOrDefaultAsync(x => x.Id == employee.Id);
            if (existing != null) { return BadRequest("Employee already exists"); }

            employee.StaffSchedule = await _unitOfWork.StaffScheduleRepository.SingleOrDefaultAsync(x => x.Id == employee.StaffSchedule.Id) ?? employee.StaffSchedule;
            employee.WorkSchedule = await _unitOfWork.WorkScheduleRepository.SingleOrDefaultAsync(x => x.Id == employee.WorkSchedule.Id) ?? employee.WorkSchedule;
            employee.Organization = await _unitOfWork.OrganizationRepository.SingleOrDefaultAsync(x => x.Id == employee.Organization.Id) ?? employee.Organization;
            var department = await _unitOfWork.DepartmentRepository.SingleOrDefaultAsync(x => x.Id == employee.Department.Id);
            if (department == null) 
            {
                await _unitOfWork.DepartmentRepository.InsertAsync(employee.Department);
            }
            employee.Department = department ?? employee.Department;
            employee.TypeOfEmployment   = await _unitOfWork.TypeOfEmploymentRepository.SingleOrDefaultAsync(x => x.Name == "Основное место работы"); // TODO: поправить получение вида занятости

            await _unitOfWork.EmployeeRepository.InsertAsync(employee);
            await _unitOfWork.SaveAsync();

            return Ok(employee.Id);
        }
    }
}
