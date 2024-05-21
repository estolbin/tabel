using System.Reflection;
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
            //if (dbContext.Employees.Any()) return;
            var te = dbContext.TypeEmployments.FirstOrDefault(x => x.Name == "РВ") ?? new TypeOfWorkingTime("РВ", "Рабочее время");
            var te1 = dbContext.TypeEmployments.FirstOrDefault(x => x.Name == "ВХ") ?? new TypeOfWorkingTime("ВХ", "Рабочее время");
            dbContext.TypeEmployments.Add(te);
            dbContext.TypeEmployments.Add(te1);
            try
            {

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            Guid orgGuid = new Guid("C5DCDC10-1AD6-4F76-AC8A-BE6E4F169AAA");
            Guid depGuid = new Guid("5AFEFC11-D0D6-4D1A-AEAB-7607CD8E2C04");

            TypeOfEmployment mainType = new TypeOfEmployment() { Name = "Основное место работы" };
            TypeOfEmployment addType = new TypeOfEmployment() { Name = "Внутреннее совместительство" };

            dbContext.TypeOfEmployments.Add(mainType);
            dbContext.TypeOfEmployments.Add(addType);

            var org = new Organization()
            {
                Id = orgGuid,
                Name = "Тестовая организация",
            };
            dbContext.Organizations.Add(org);


            var dep = new Department("Тестовое подразделение", org);
            var workSchedule = new WorkSchedule()
            {
                Id = depGuid,
                Name = "Тестовая пятидневка"
            };
            workSchedule.HoursByDayNumbers = WorkSchedule.GetDefaultList(workSchedule);

            foreach (var day in workSchedule.HoursByDayNumbers)
            {
                if (day.TypeOfWorkingTime.Name == "РВ") day.TypeOfWorkingTime = te;
                else if (day.TypeOfWorkingTime.Name == "ВХ") day.TypeOfWorkingTime = te1;
            }


            var staff = new StaffSchedule(org, dep, workSchedule, "Штатное расписание");


            var name = new EmployeeName("Иванов Иван Иванович");
            var emp = new Employee(name, org, dep, staff);
            //emp.TypeEmployment = te;
            emp.WorkSchedule = workSchedule;
            emp.TypeOfEmployment = addType;

            dbContext.Organizations.Add(org);
            dbContext.Departments.Add(dep);
            dbContext.WorkSchedules.Add(workSchedule);
            dbContext.StaffSchedules.Add(staff);
            //dbContext.EmployeeNames.Add(name);
            dbContext.Employees.Add(emp);

            var per = new TimeShiftPeriod("Новый период", DateTime.Parse("01.04.2024"), DateTime.Parse("15.04.2024"));
            dbContext.TimeShiftPeriods.Add(per);

            List<TimeShift> tsper = new();
            for (DateTime date = per.Start; date <= per.End; date = date.AddDays(1))
            {
                var t = new TimeShift(per, emp, date);
                if (t.TypeEmployment.Name == "РВ") t.TypeEmployment = te;
                else if (t.TypeEmployment.Name == "ВХ") t.TypeEmployment = te1;
                tsper.Add(t);
            }
            dbContext.TimeShifts.AddRange(tsper);

            try
            {

                dbContext.SaveChanges();
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // second employee

            var org1 = new Organization() { Id = new Guid(), Name = "ООО Ромашка" };
            var dep1 = new Department() { Id = new Guid(), Name = "Дирекция", Organization = org1 };

            dbContext.Organizations.Add(org1);
            dbContext.Departments.Add(dep1);

            var staff1 = new StaffSchedule() { Id = new Guid(), Name = "Test", Organization = org1, Department = dep1, WorkSchedule = workSchedule };
            var name1 = new EmployeeName("Сидоров Макар Ромашкович");
            var emp1 = new Employee(name1, org1, dep1, staff1);
            //emp1.TypeEmployment = te;
            emp1.WorkSchedule = workSchedule;
            emp1.TypeOfEmployment = mainType;

            dbContext.StaffSchedules.Add(staff1);
            //dbContext.EmployeeNames.Add(name1);
            dbContext.Employees.Add(emp1);

            List<TimeShift> tsper1 = new();
            for (DateTime date = per.Start; date <= per.End; date = date.AddDays(1))
            {
                var t = new TimeShift(per, emp1, date);
                if (t.TypeEmployment.Name == "РВ") t.TypeEmployment = te;
                else if (t.TypeEmployment.Name == "ВХ") t.TypeEmployment = te1;

                tsper1.Add(t);
            }
            dbContext.TimeShifts.AddRange(tsper1);
            dbContext.SaveChanges();

            // third employee

            //org1 = new Organization() { Id = new Guid(), Name = "ООО Ромашка" };
            //var dep1 = new Department() { Id = new Guid(), Name = "Дирекция", Organization = org1 };

            //dbContext.Organizations.Add(org1);
            //dbContext.Departments.Add(dep1);

            staff1 = new StaffSchedule() { Id = new Guid(), Name = "Главный бухгалтер/Администрация", Organization = org1, Department = dep1, WorkSchedule = workSchedule };
            name1 = new EmployeeName("Романова Виктория Александровна");
            emp1 = new Employee(name1, org1, dep1, staff1);
            //emp1.TypeEmployment = te;
            emp1.WorkSchedule = workSchedule;
            emp1.TypeOfEmployment = mainType;

            dbContext.StaffSchedules.Add(staff1);
            //dbContext.EmployeeNames.Add(name1);
            dbContext.Employees.Add(emp1);

            tsper1 = new();
            for (DateTime date = per.Start; date <= per.End; date = date.AddDays(1))
            {
                var t = new TimeShift(per, emp1, date);
                if (t.TypeEmployment.Name == "РВ") t.TypeEmployment = te;
                else if (t.TypeEmployment.Name == "ВХ") t.TypeEmployment = te1;

                tsper1.Add(t);
            }
            dbContext.TimeShifts.AddRange(tsper1);
            dbContext.SaveChanges();

            // fourth employee
            //org1 = new Organization() { Id = new Guid(), Name = "ООО Ромашка" };
            dep1 = new Department() { Id = new Guid(), Name = "Бухгалтерия", Organization = org1 };

            //dbContext.Organizations.Add(org1);
            dbContext.Departments.Add(dep1);

            staff1 = new StaffSchedule() { Id = new Guid(), Name = "Бухгалтер-расчетчик/Бухгалтерия", Organization = org1, Department = dep1, WorkSchedule = workSchedule };
            name1 = new EmployeeName("Константинова Зинаида Вячеславовна");
            emp1 = new Employee(name1, org1, dep1, staff1);
            //emp1.TypeEmployment = te;
            emp1.WorkSchedule = workSchedule;
            emp1.TypeOfEmployment = mainType;

            dbContext.StaffSchedules.Add(staff1);
            //dbContext.EmployeeNames.Add(name1);
            dbContext.Employees.Add(emp1);

            tsper1 = new();
            for (DateTime date = per.Start; date <= per.End; date = date.AddDays(1))
            {
                var t = new TimeShift(per, emp1, date);
                if (t.TypeEmployment.Name == "РВ") t.TypeEmployment = te;
                else if (t.TypeEmployment.Name == "ВХ") t.TypeEmployment = te1;

                tsper1.Add(t);
            }
            dbContext.TimeShifts.AddRange(tsper1);
            dbContext.SaveChanges();


        }
    }
}
