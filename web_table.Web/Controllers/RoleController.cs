using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_table.Web.Controllers
{
    [Authorize(Policy ="AdminOnly")]
    public class RoleController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public RoleController()
        {
              _unitOfWork = new UnitOfWork();
        }


        public async Task<IActionResult> Index()
        {
            var roles = await _unitOfWork.RoleRepository.GetAllAsync();
            return View(roles);
        }

        public async Task<IActionResult> Edit(string Name)
        {
            var role = await _unitOfWork.RoleRepository.SingleOrDefaultAsync(x => x.Name == Name);
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm]Role role)
        {
            await _unitOfWork.RoleRepository.UpdateAsync(role);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
