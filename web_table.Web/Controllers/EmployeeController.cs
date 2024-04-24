using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;

namespace web_table.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ITimeShiftService _service;

        public EmployeeController(ITimeShiftService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var emp = await _service.GetEmployeeById(id);
            return View(emp);
        }
    }
}
