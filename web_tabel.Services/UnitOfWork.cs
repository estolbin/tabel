using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_tabel.Domain;
using web_tabel.Services.TimeShiftContext;

namespace web_tabel.Services
{
    public class UnitOfWork : IDisposable
    {
        private readonly TimeShiftDBContext _context = new();
        private GenericRepository<Department> _departmentRepository;
        private GenericRepository<Employee> _employeeRepository;
        private GenericRepository<EmployeeName> _employeeNameRepository;
        private GenericRepository<Organization> _organizationRepository;
        private GenericRepository<StaffSchedule> _staffScheduleRepository;
        private GenericRepository<TimeShift> _timeShiftRepository;
        private GenericRepository<TimeShiftPeriod> _timeShiftPeriodRepository;
        private GenericRepository<TypeOfEmployment> _typeOfEmploymentRepository;
        private GenericRepository<TypeOfWorkingTime> _typeOfWorkingTimeRepository;
        private GenericRepository<WorkSchedule> _workScheduleRepository;
        private GenericRepository<WorkSchedulleHours> _workSchedulleHoursRepository;
        private GenericRepository<WorkingCalendar> _workingCalendarRepository;

        public GenericRepository<Department> DepartmentRepository => _departmentRepository ??= new(_context);
        public GenericRepository<Employee> EmployeeRepository => _employeeRepository ??= new(_context);
        public GenericRepository<EmployeeName> EmployeeNameRepository => _employeeNameRepository ??= new(_context);
        public GenericRepository<Organization> OrganizationRepository => _organizationRepository ??= new(_context);
        public GenericRepository<StaffSchedule> StaffScheduleRepository => _staffScheduleRepository ??= new(_context);
        public GenericRepository<TimeShift> TimeShiftRepository => _timeShiftRepository ??= new(_context);
        public GenericRepository<TimeShiftPeriod> TimeShiftPeriodRepository => _timeShiftPeriodRepository ??= new(_context);
        public GenericRepository<TypeOfEmployment> TypeOfEmploymentRepository => _typeOfEmploymentRepository ??= new(_context);
        public GenericRepository<TypeOfWorkingTime> TypeOfWorkingTimeRepository => _typeOfWorkingTimeRepository ??= new(_context);
        public GenericRepository<WorkSchedule> WorkScheduleRepository => _workScheduleRepository ??= new(_context);
        public GenericRepository<WorkSchedulleHours> WorkSchedulleHoursRepository => _workSchedulleHoursRepository ??= new(_context);
        public GenericRepository<WorkingCalendar> WorkingCalendarRepository => _workingCalendarRepository ??= new(_context);


        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
