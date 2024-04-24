using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;

namespace web_table.Web.Controllers
{
    [Controller]
    public class PeriodController : Controller
    {
        private readonly ITimeShiftService _service;

        public PeriodController(ITimeShiftService service)
        {
            _service = service;
        }

        /// <summary>
        /// Список всех периодов
        /// </summary>
        /// <returns></returns>
        // GET: PeriodController
        public IActionResult Index()
        {
            var periods = _service.GetAllPeriods();
            return View(periods.Result);
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

            _service.AddPeriod(period);

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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PeriodController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: PeriodController/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (_service.RemovePeriodById(id))
            {
                ViewBag.Message = $"Период с идентификатором {id} удален.";
            }
            else
            {
                ViewBag.Message = "Ошибка удаления";
            }
            return View();
        }

        // POST: PeriodController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
