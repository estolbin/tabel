using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_table.Web.Controllers
{
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

        public async Task<IActionResult> FillPeriod(string periodId)
        {
            var period = _unitOfWork.TimeShiftPeriodRepository.SingleOrDefault(x => x.Id == new Guid(periodId));
            var employees = _unitOfWork.EmployeeRepository.GetAll();

            
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
                int numberDayOfCycle;
                if (workSheet.IsWeekly)
                {
                    numberDayOfCycle = (int)period.Start.DayOfWeek;
                } 
                else
                {
                    numberDayOfCycle = (period.Start - refDate).Days % numDaysInCycle + 1;

                }

                var curDate = period.Start;
                while (curDate <= period.End)
                {
                    var ts = new TimeShift(period, employee, curDate);
                    ts.HoursPlanned = daysInCycleList.FirstOrDefault(x => x.DayNumber == numberDayOfCycle).Hours;
                    ts.TypeEmployment = daysInCycleList.FirstOrDefault(x => x.DayNumber == numberDayOfCycle).TypeOfWorkingTime; 
                    if (ts.HoursPlanned == 0 && ts.TypeEmployment.Name == "Я")
                    {
                        ts.TypeEmployment = _unitOfWork.TypeOfWorkingTimeRepository.SingleOrDefault(x => x.Name == "В");
                    }

                    listTimeShift.Add(ts);
                    curDate = curDate.AddDays(1);

                    numberDayOfCycle++;
                    if (numberDayOfCycle > numDaysInCycle) numberDayOfCycle = 1;
                }

                _unitOfWork.TimeShiftRepository.AddRange(listTimeShift);
            }
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

   
    }
}
