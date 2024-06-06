using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_tabel.API.Controllers
{
    [Route("api/EmployeeState")]
    [ApiController]
    public class EmployeeStateController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public EmployeeStateController()
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _unitOfWork.EmployeeStateRepository.GetAllAsync());
        }

        [HttpPost("AddEmployeeState")]
        public async Task<IActionResult> AddEmployeeState([FromBody] EmployeeStateDto employeeState)
        {
            Employee exist = await _unitOfWork.EmployeeRepository.SingleOrDefaultAsync(x => x.Id == employeeState.EmployeeId);
            if (exist == null)
            {
                return BadRequest("Employee not found");
            }

            var existState = await _unitOfWork.EmployeeConditionRepository.SingleOrDefaultAsync(x => x.Name == employeeState.ConditionName);
            if (existState == null) return BadRequest("EmployeeCondition not found");

            var check = await _unitOfWork.EmployeeStateRepository.GetAsync(x => x.Employee == exist &&
                    x.Condition == existState &&
                    x.StartDate.Day == employeeState.StartDate.Day &&
                    x.EndDate.Day == employeeState.EndDate.Day);

            if (check.Any())
            {
                return BadRequest("EmployeeState already exists");
            }


            var empState = new EmployeeState
            {
                EmployeeId = employeeState.EmployeeId,
                ConditionName = employeeState.ConditionName,
                StartDate = employeeState.StartDate,
                EndDate = employeeState.EndDate,
            };
            
            var list = await _unitOfWork.EmployeeStateRepository.GetAllAsync();
            empState.Id = list.ToList().Count > 0 ? list.ToList().Max(x => x.Id) + 1 : 1;

            await _unitOfWork.EmployeeStateRepository.InsertAsync(empState);
            await _unitOfWork.SaveAsync();

            for (DateTime d = empState.StartDate; d <= empState.EndDate; d = d.AddDays(1))
            {
                var rec = await _unitOfWork.TimeShiftRepository.SingleOrDefaultAsync(x => x.Employee.Id == empState.EmployeeId && x.WorkDate.Day == d.Day);

                var newValue = await _unitOfWork.EmployeeConditionRepository.SingleOrDefaultAsync(c => c.Name == empState.ConditionName);
                if (newValue == null) continue;
                var newType = newValue.TypeOfWorkingTime;
                var isNew = (rec == null);

                TimeShift timeShift;
                if (rec == null)
                {
                    timeShift = new TimeShift();
                }
                else
                {
                    timeShift = rec;
                }

                timeShift.TimeShiftPeriod = await TimeShiftService.GetLastPeriod(_unitOfWork);
                timeShift.Employee = empState.Employee;
                timeShift.WorkDate = d;
                timeShift.WorkSchedule = empState.Employee.WorkSchedule;
                timeShift.TypeEmploymentPlanned = newType;

                if (isNew)
                    await _unitOfWork.TimeShiftRepository.InsertAsync(timeShift);
                else
                    await _unitOfWork.TimeShiftRepository.UpdateAsync(timeShift);

                await _unitOfWork.SaveAsync();
            }

            return Ok("Success");
        }

    }

    public class EmployeeStateDto
    {
        public Guid EmployeeId { get; set; }
        public string ConditionName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
