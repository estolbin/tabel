using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_tabel.API.Controllers
{
    [Route("api/StaffSchedule")]
    [ApiController]
    public class StaffScheduleController : ControllerBase
    {
        private UnitOfWork _unitOfWork;

        public StaffScheduleController() 
        {
            _unitOfWork = new UnitOfWork(); 
        }

        [HttpGet("GetAllStaffSchedule")]
        public Task<IActionResult> GetAllStaffSchedule()
        {
            return Task.FromResult<IActionResult>(Ok(_unitOfWork.StaffScheduleRepository.GetAll()));
        }

        [HttpPost("AddStaffSchedule")]
        public Task<IActionResult> AddStaffSchedule([FromBody] StaffSchedule staffSchedule)
        {
            if (_unitOfWork.StaffScheduleRepository.SingleOrDefault(x => x.Id == staffSchedule.Id) != null) { return Task.FromResult<IActionResult>(BadRequest("StaffSchedule already exists")); }

            staffSchedule.Department = _unitOfWork.DepartmentRepository.SingleOrDefault(x => x.Id == staffSchedule.Department.Id) ?? staffSchedule.Department;
            staffSchedule.Organization = _unitOfWork.OrganizationRepository.SingleOrDefault(x => x.Id == staffSchedule.Organization.Id) ?? staffSchedule.Organization;
            staffSchedule.WorkSchedule = _unitOfWork.WorkScheduleRepository.SingleOrDefault(x => x.Id == staffSchedule.WorkSchedule.Id) ?? staffSchedule.WorkSchedule;

            _unitOfWork.StaffScheduleRepository.Insert(staffSchedule);
            _unitOfWork.Save();

            return Task.FromResult<IActionResult>(Ok("Success"));
        }
    }
}
