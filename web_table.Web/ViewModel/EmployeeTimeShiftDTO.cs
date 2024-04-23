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

        public static IEnumerable<EmployeeTimeShiftDTO> ToListFromTimeShift(IEnumerable<TimeShift> timeShifts) 
        {
            List<EmployeeTimeShiftDTO > temp = new List<EmployeeTimeShiftDTO>();

            var employees = timeShifts.Select(e => e.Employee).Distinct().ToList();
            foreach (var employee in employees)
            {
                var empTS = new EmployeeTimeShiftDTO();
                empTS.EmployeeName = employee.Name.FullName;
                empTS.EmployeeId = employee.Id;
                empTS.PositionName = employee.StaffSchedule.Position.Name;
                empTS.WorkScheduleName = employee.WorkSchedule.Name;
                empTS.OrganizationId = employee.Organization.Id.ToString();
                empTS.DepartmentId = employee.Department.Id.ToString();

                empTS.TypeOfEmp = employee.TypeOfEmployment.Name;

                var res = timeShifts.OrderBy(x=>x.WorkDate).Where(e => e.Employee == employee).ToList();
                foreach (var item in res)
                {
                    empTS.HoursWorked.Add(item.HoursWorked);
                    empTS.HoursPlanned.Add(item.HoursPlanned);
                    empTS.Dates.Add(item.WorkDate);
                    empTS.Types.Add(item.TypeEmployment);
                }
                temp.Add(empTS);
            }
            return temp;
        }
    }
}
