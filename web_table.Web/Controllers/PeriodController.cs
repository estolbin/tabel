using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_table.Web.Controllers
{
    [Authorize(Policy="AdminOnly")]
    [Controller]
    public class PeriodController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public PeriodController()
        {
            _unitOfWork = new UnitOfWork();
        }

        private async Task<Guid> GetGuidFromSession()
        {
            var periodId = HttpContext.Session.GetString("SessionPeriodId");
            // if periodId == null, we must take last period
            var dbPeriod = _unitOfWork.TimeShiftPeriodRepository.SingleOrDefault(x => x.Name != null);
            if (periodId == null)
            {
                periodId = dbPeriod.Id.ToString();
            };
            return periodId == null ? Guid.Empty : new Guid(periodId);
        }

        /// <summary>
        /// Список всех периодов
        /// </summary>
        /// <returns></returns>
        // GET: PeriodController
        public IActionResult Index()
        {
            var periods = _unitOfWork.TimeShiftPeriodRepository.GetAll();
            return View(periods);
        }

        // GET: PeriodController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PeriodController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: PeriodController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TimeShiftPeriod period)
        {

            //_service.AddPeriod(period);
            _unitOfWork.TimeShiftPeriodRepository.Insert(period);
            _unitOfWork.Save();

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PeriodController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var period = _unitOfWork.TimeShiftPeriodRepository.SingleOrDefault(x => x.Id == id);
            return View(period);
        }

        // POST: PeriodController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TimeShiftPeriod period, Guid id)
        {
            try
            {
                _unitOfWork.TimeShiftPeriodRepository.Update(period);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task<TypeOfWorkingTime> GetTypeOfWorkingTime(string name)
        {
            return await _unitOfWork.TypeOfWorkingTimeRepository.SingleOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IActionResult> FillPeriod(string periodId)
        {
            var period = await _unitOfWork.TimeShiftPeriodRepository.SingleOrDefaultAsync(x => x.Id == new Guid(periodId));
            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            
            var workCalendar = await _unitOfWork.WorkingCalendarRepository.GetAsync(x => x.Year == period.Start.Year);

            var weekendDayType = await GetTypeOfWorkingTime("В");
            var celebrateDayType = await GetTypeOfWorkingTime("РВ");

            foreach (var employee in employees)
            {
                List<TimeShift> listTimeShift = new List<TimeShift>();


                var workSheet = employee.WorkSchedule;
                if (workSheet == null)
                    continue;

                // список дней в цикле
                var daysInCycleList = workSheet.HoursByDayNumbers.ToList();
                var numDaysInCycle = daysInCycleList.Count();

                var refDate = workSheet.ReferenceDate ?? new DateTime(period.Start.Year, 1, 1);
                int numberDayOfCycle = GetNumberDayOfCycle(period, workSheet, numDaysInCycle, refDate);

                var curDate = period.Start;
                while (curDate <= period.End)
                {

                    // день по производственному календарю
                    var calendarDay = workCalendar.FirstOrDefault(x => x.Date == curDate);

                    var existed = await _unitOfWork.TimeShiftRepository.GetAsync(x => x.Employee.Id == employee.Id && x.WorkDate == curDate);
                    // TODO: уточнить, что делаем если перезаполняют табель
                    if (existed.Any())
                        continue;

                    var ts = new TimeShift(period, employee, curDate);
                    ts.HoursPlanned = GetHoursPlanned(daysInCycleList, numberDayOfCycle);
                    ts.TypeEmploymentPlanned = daysInCycleList.FirstOrDefault(x => x.DayNumber == numberDayOfCycle).TypeOfWorkingTime;
                    if (ts.HoursPlanned == 0 && ts.TypeEmploymentPlanned.Name == "Я")
                    {
                        ts.TypeEmploymentPlanned = weekendDayType;
                    }
                    else if (ts.TypeEmploymentPlanned.Name == "Я" && workSheet.IsWeekly) // для графиков такое условие не работает
                    {
                        if (calendarDay.Type == DayType.Celebrate || calendarDay.Type == DayType.Weekend)
                        { 
                            ts.TypeEmploymentPlanned = weekendDayType;
                            ts.HoursPlanned = 0;
                        }

                    }

                    listTimeShift.Add(ts);
                    curDate = curDate.AddDays(1);

                    numberDayOfCycle = GetNextDayInCycle(numDaysInCycle, numberDayOfCycle);
                }

                _unitOfWork.TimeShiftRepository.AddRange(listTimeShift);
            }
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        private static int GetNextDayInCycle(int numDaysInCycle, int numberDayOfCycle)
        {
            numberDayOfCycle++;
            if (numberDayOfCycle > numDaysInCycle) numberDayOfCycle = 1;
            return numberDayOfCycle;
        }

        private float GetHoursPlanned(List<WorkSchedulleHours> daysInCycleList, int numberDayOfCycle) => daysInCycleList.FirstOrDefault(x => x.DayNumber == numberDayOfCycle).Hours;

        private static int GetNumberDayOfCycle(TimeShiftPeriod period, WorkSchedule workSheet, int numDaysInCycle, DateTime refDate)
        {
            if (workSheet.IsWeekly)
            {
                return (int)period.Start.DayOfWeek;
            }
            else
            {
                return (period.Start - refDate).Days % numDaysInCycle + 1;

            }
        }

    }
}
