﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using web_tabel.Domain;
using web_tabel.Services.TimeShiftContext;

namespace web_table.Web
{
    internal class DbInitializer
    {
        internal static void Initialize(TimeShiftDBContext dbContext)
        {
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
            dbContext.Database.EnsureCreated();
            if (dbContext.Employees.Any()) return;

            Guid orgGuid = new Guid("C5DCDC10-1AD6-4F76-AC8A-BE6E4F169AAA");
            Guid depGuid = new Guid("5AFEFC11-D0D6-4D1A-AEAB-7607CD8E2C04");

            var org = new Organization()
            {
                Id = orgGuid,
                Name = "Тестовая организация",
            };
            dbContext.Organizations.Add(org);


            var dep = new Department("Тестовое подразделение", org);
            var pos = new Position("Должность", org, dep);
            var workSchedule = new WorkSchedule()
            {
                Id = depGuid,
                Name = "Тестовая пятидневка",
                HoursOfWork = 40f
            };
            var staff = new StaffSchedule(org, dep, pos, workSchedule, "Штатное расписание");

            var te = new TypeEmployment("РВ", "Рабочее время");

            var name = new EmployeeName("Иванов Иван Иванович");
            var emp = new Employee(name, org, dep, staff);
            emp.TypeEmployment  = te;
            emp.WorkSchedule = workSchedule;

            dbContext.Organizations.Add(org);
            dbContext.Departments.Add(dep);
            dbContext.Positions.Add(pos);
            dbContext.WorkSchedules.Add(workSchedule);
            dbContext.StaffSchedules.Add(staff);
            dbContext.EmployeeNames.Add(name);
            dbContext.Employees.Add(emp);

            var per = new TimeShiftPeriod("Новый период", DateTime.Parse("01.04.2024"), DateTime.Parse("15.04.2024"));
            dbContext.TimeShiftPeriods.Add(per);

            List<TimeShift> tsper = new();
            for (DateTime date = per.Start; date <= per.End; date = date.AddDays(1))
            {
                var t = new TimeShift(per, emp, date);
                tsper.Add(t);
            }
            dbContext.TimeShifts.AddRange(tsper);

            dbContext.SaveChanges();

            var org1 = new Organization() { Id = new Guid(), Name = "ООО Ромашка" };
            var dep1 = new Department() { Id = new Guid(), Name = "Дирекция", Organization = org1 };
            var pos1 = new Position() { Id = new Guid(), Name = "ДиреХтор", Organization = org1, Department = dep1 };

            dbContext.Organizations.Add(org1);
            dbContext.Departments.Add(dep1);
            dbContext.Positions.Add(pos1);

            var staff1 = new StaffSchedule() { Id = new Guid(), Name = "Test", Organization = org1, Department = dep1, Position = pos1, WorkSchedule = workSchedule };
            var name1 = new EmployeeName("Сидоров Макар Ромашкович");
            var emp1 = new Employee(name1, org1, dep1, staff1);
            emp1.TypeEmployment = te;
            emp1.WorkSchedule = workSchedule;

            dbContext.StaffSchedules.Add(staff1);
            dbContext.EmployeeNames.Add(name1);
            dbContext.Employees.Add(emp1);

            List<TimeShift> tsper1 = new();
            for (DateTime date = per.Start; date <= per.End; date = date.AddDays(1))
            {
                var t = new TimeShift(per, emp1, date);
                tsper1.Add(t);
            }
            dbContext.TimeShifts.AddRange(tsper1);
            dbContext.SaveChanges();
        }
    }
}
