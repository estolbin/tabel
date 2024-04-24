using web_tabel.Domain;

namespace web_table.Web.ViewModel
{
    public class EmployeeTimeShiftDTO
    {
        public string EmployeeName { get; set; }
        public Guid EmployeeId { get; set; }
        public List<float> HoursPlanned { get; set; } = new List<float>();
        public List<float> HoursWorked { get; set; } = new List<float>();
        public List<DateTime> Dates { get; set; } = new List<DateTime>();

        public string OrganizationId { get; set; }
        public string DepartmentId { get; set; }

        public List<TypeOfWorkingTime> Types { get; set; } = new List<TypeOfWorkingTime>();

        public string PositionName {  get; set; }

        public string WorkScheduleName { get; set; }

        public string TypeOfEmp { get; set; }

        public EmployeeTimeShiftDTO() { }

        public async static Task<IEnumerable<EmployeeTimeShiftDTO>> ToListFromTimeShift(IEnumerable<TimeShift> timeShifts) 
        {
            var employeeTimeShiftDTOs = new List<EmployeeTimeShiftDTO>();

            // TODO: need to include all fields
            var distinctEmployees = timeShifts.Select(ts => ts.Employee).Distinct().ToList();
            await Task.WhenAll(distinctEmployees.Select(async employee =>
            {
                var employeeTimeShiftDTO = new EmployeeTimeShiftDTO
                {

                    EmployeeName = employee.Name.FullName,
                    EmployeeId = employee.Id,
                    PositionName = employee.StaffSchedule.Position.Name,
                    WorkScheduleName = employee.WorkSchedule.Name,
                    OrganizationId = employee.Organization.Id.ToString(),
                    DepartmentId = employee.Department.Id.ToString(),
                    TypeOfEmp = employee.TypeOfEmployment.Name
                };

                var employeeTimeShifts = timeShifts.Where(ts => ts.Employee == employee).OrderBy(ts => ts.WorkDate).ToList();
                employeeTimeShiftDTO.HoursWorked.AddRange(employeeTimeShifts.Select(ts => ts.HoursWorked));
                employeeTimeShiftDTO.HoursPlanned.AddRange(employeeTimeShifts.Select(ts => ts.HoursPlanned));
                employeeTimeShiftDTO.Dates.AddRange(employeeTimeShifts.Select(ts => ts.WorkDate));
                employeeTimeShiftDTO.Types.AddRange(employeeTimeShifts.Select(ts => ts.TypeEmployment));

                employeeTimeShiftDTOs.Add(employeeTimeShiftDTO);
            }));
            return employeeTimeShiftDTOs;
        }
    }
}
