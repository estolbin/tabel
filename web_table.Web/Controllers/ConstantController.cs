using Microsoft.AspNetCore.Mvc;
using web_tabel.Services;

namespace web_table.Web.Controllers
{
    public class ConstantController : Controller
    {

        public async Task<IActionResult> LockDate()
        {
            var date = await Constants.GetConstantValue<DateTime>("LockDate");
            return View(date);
        }

        [HttpPost]
        public async Task<IActionResult> LockDate(DateTime lockDate)
        {
            await Constants.SetConstantValue<DateTime>("LockDate", lockDate);
            return RedirectToAction(nameof(LockDate));
        }
    }
}
