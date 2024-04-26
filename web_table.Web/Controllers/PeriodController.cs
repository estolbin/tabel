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
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

   
    }
}
