﻿using web_tabel.Domain;

namespace web_table.Web.ViewModel
{
    public class EmployeeTimeShiftViewModel
    {
        public string EmployeeName { get; set; }
        public Guid EmployeeId { get; set; }
        public List<float> HoursPlanned { get; set; } = new List<float>();
        public List<float> HoursWorked { get; set; } = new List<float>();
        public List<DateTime> Dates { get; set; } = new List<DateTime>();

        public float HoursPlannedSum
        {
            get
            {
                return HoursPlanned.Sum();
            }
        }
        public float HoursWorkedSum
        {
            get
            {
                return HoursWorked.Sum();
            }
        }

        public string OrganizationId { get; set; }
        public string DepartmentId { get; set; }

        public string OrganizationName { get; set; }
        public string DepartmentName { get; set; }

        public List<TypeOfWorkingTime> TypesPlanned { get; set; } = new List<TypeOfWorkingTime>();
        public List<TypeOfWorkingTime> TypesWorked { get; set; } = new List<TypeOfWorkingTime>();

        public List<string> ColorTypes { get; set; } = new List<string>();

        public string PositionName { get; set; }

        public string WorkScheduleName { get; set; }

        public string TypeOfEmp { get; set; }


        public string Period {  get; set; }

        public EmployeeTimeShiftViewModel() { }

        public async static Task<IEnumerable<EmployeeTimeShiftViewModel>> ToListFromTimeShift(IEnumerable<TimeShift> timeShifts)
        {
            var employeeTimeShifts = new List<EmployeeTimeShiftViewModel>();

            // TODO: select distinct employees from list
            IEnumerable<Employee> employees = timeShifts.Select(ts => ts.Employee).Distinct();
            foreach (var employee in employees)
            {
                var employeeTimeShift = new EmployeeTimeShiftViewModel
                {
                    EmployeeName = employee.Name.FullName,
                    EmployeeId = employee.Id,
                    PositionName = employee.StaffSchedule.Name,
                    WorkScheduleName = employee.WorkSchedule.Name,
                    OrganizationId = employee.Organization.Id.ToString(),
                    DepartmentId = employee.Department.Id.ToString(),
                    TypeOfEmp = employee.TypeOfEmployment.Name,
                    Period = timeShifts.FirstOrDefault(ts => ts.Employee == employee)?.TimeShiftPeriod.Name,
                    OrganizationName = employee.Organization.Name,
                    DepartmentName = employee.Department.Name
                };

                var employeeTimeShiftsList = timeShifts.Where(ts => ts.Employee == employee).OrderBy(ts => ts.WorkDate);
                employeeTimeShift.HoursWorked.AddRange(employeeTimeShiftsList.Select(ts => ts.HoursWorked));
                employeeTimeShift.HoursPlanned.AddRange(employeeTimeShiftsList.Select(ts => ts.HoursPlanned));
                employeeTimeShift.Dates.AddRange(employeeTimeShiftsList.Select(ts => ts.WorkDate));
                employeeTimeShift.TypesPlanned.AddRange(employeeTimeShiftsList.Select(ts => ts.TypeEmploymentPlanned));
                employeeTimeShift.TypesWorked.AddRange(employeeTimeShiftsList.Select(ts => ts.TypeEmploymentWorked));
                employeeTimeShift.ColorTypes.AddRange(employeeTimeShiftsList.Select(ts => ts.TypeEmploymentPlanned?.ColorText));

                employeeTimeShifts.Add(employeeTimeShift);
            };

            return employeeTimeShifts;

        }
    }
}
