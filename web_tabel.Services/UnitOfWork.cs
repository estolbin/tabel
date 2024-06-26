﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_tabel.Domain;
using web_tabel.Domain.UserFilters;
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
        private GenericRepository<EmployeeCondition> _employeeConditionRepository;
        private GenericRepository<EmployeeState> _employeeStateRepository;
        private GenericRepository<TypeOfWorkingTimeRules> _typeOfWorkingTimeRulesRepository;
        private GenericRepository<AppUser> _userRepository;
        private GenericRepository<Role> _roleRepository;
        private GenericRepository<Filter> _filterRepository;
        private GenericRepository<Constant> _constantRepository;
        private GenericRepository<ConfirmedPeriod> _confirmedPeriodRepository;

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
        public GenericRepository<EmployeeCondition> EmployeeConditionRepository => _employeeConditionRepository ??= new(_context);
        public GenericRepository<EmployeeState> EmployeeStateRepository => _employeeStateRepository ??= new(_context);
        public GenericRepository<TypeOfWorkingTimeRules> TypeOfWorkingTimeRulesRepository => _typeOfWorkingTimeRulesRepository ??= new(_context);
        public GenericRepository<AppUser> UserRepository => _userRepository ??= new(_context);
        public GenericRepository<Role> RoleRepository => _roleRepository ??= new(_context);
        public GenericRepository<Filter> FilterRepository => _filterRepository ??= new(_context);
        public GenericRepository<Constant> ConstantRepository => _constantRepository ??= new(_context);
        public GenericRepository<ConfirmedPeriod> ConfirmedPeriodRepository => _confirmedPeriodRepository ??= new(_context);

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
