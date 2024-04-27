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

        public string PositionName { get; set; }

        public string WorkScheduleName { get; set; }

        public string TypeOfEmp { get; set; }

        public string Period {  get; set; }

        public EmployeeTimeShiftDTO() { }

        public async static Task<IEnumerable<EmployeeTimeShiftDTO>> ToListFromTimeShift(IEnumerable<TimeShift> timeShifts)
        {
            var employeeTimeShifts = new List<EmployeeTimeShiftDTO>();

            // TODO: select distinct employees from list
            IEnumerable<Employee> employees = timeShifts.Select(ts => ts.Employee).Distinct();
            foreach (var employee in employees)
            {
                var employeeTimeShift = new EmployeeTimeShiftDTO
                {
                    EmployeeName = employee.Name.FullName,
                    EmployeeId = employee.Id,
                    PositionName = employee.StaffSchedule.Name,
                    WorkScheduleName = employee.WorkSchedule.Name,
                    OrganizationId = employee.Organization.Id.ToString(),
                    DepartmentId = employee.Department.Id.ToString(),
                    TypeOfEmp = employee.TypeOfEmployment.Name,
                    Period = timeShifts.FirstOrDefault(ts => ts.Employee == employee)?.TimeShiftPeriod.Name
                };

                var employeeTimeShiftsList = timeShifts.Where(ts => ts.Employee == employee).OrderBy(ts => ts.WorkDate);
                employeeTimeShift.HoursWorked.AddRange(employeeTimeShiftsList.Select(ts => ts.HoursWorked));
                employeeTimeShift.HoursPlanned.AddRange(employeeTimeShiftsList.Select(ts => ts.HoursPlanned));
                employeeTimeShift.Dates.AddRange(employeeTimeShiftsList.Select(ts => ts.WorkDate));
                employeeTimeShift.Types.AddRange(employeeTimeShiftsList.Select(ts => ts.TypeEmployment));

                employeeTimeShifts.Add(employeeTimeShift);
            };

            return employeeTimeShifts;

        }
    }
}
