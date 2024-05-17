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
        public Task<IActionResult> GetAllEmployees()
        {
            return Task.FromResult<IActionResult>(Ok(_unitOfWork.EmployeeRepository.GetAll()));
        }

        [HttpPost("AddEmployee")]
        public Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {

            employee.Name.Id = employee.Id;
            var existing = _unitOfWork.EmployeeRepository.SingleOrDefault(x => x.Id == employee.Id);
            if (existing != null) { return Task.FromResult<IActionResult>(BadRequest("Employee already exists")); }

            employee.StaffSchedule = _unitOfWork.StaffScheduleRepository.SingleOrDefault(x => x.Id == employee.StaffSchedule.Id) ?? employee.StaffSchedule;
            employee.WorkSchedule = _unitOfWork.WorkScheduleRepository.SingleOrDefault(x => x.Id == employee.WorkSchedule.Id) ?? employee.WorkSchedule;
            employee.Organization = _unitOfWork.OrganizationRepository.SingleOrDefault(x => x.Id == employee.Organization.Id) ?? employee.Organization;
            employee.Department = _unitOfWork.DepartmentRepository.SingleOrDefault(x => x.Id == employee.Department.Id) ?? employee.Department;
            employee.TypeOfEmployment   = _unitOfWork.TypeOfEmploymentRepository.SingleOrDefault(x => x.Name == "Основное место работы"); // TODO: поправить получение вида занятости

            _unitOfWork.EmployeeRepository.Insert(employee);
            _unitOfWork.Save();

            return Task.FromResult<IActionResult>(Ok(employee.Id));
        }
    }
}
