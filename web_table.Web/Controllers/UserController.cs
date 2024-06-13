using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_table.Web.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class UserController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public UserController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public async Task<IActionResult> Index()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return View(users);
        }

        public async Task<IActionResult> Create()
        {
            var model = new AppUser();
            ViewBag.Roles = (await _unitOfWork.RoleRepository.GetAllAsync()).ToSelectListItem(x => x.Name, x => x.Description, null);
            ViewBag.Filters = (await _unitOfWork.FilterRepository.GetAllAsync()).ToSelectListItem(x => x.Id, x => x.Name, null);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppUser model)
        {
            model.Role = await _unitOfWork.RoleRepository.SingleOrDefaultAsync(x => x.Name == model.RoleName);
            
            if(model.FilterId != null)
                model.Filter = await _unitOfWork.FilterRepository.SingleOrDefaultAsync(x => x.Id == model.FilterId);

            //if (ModelState.IsValid)
            //{
                await _unitOfWork.UserRepository.InsertAsync(model);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            //}
            //return View(model);
        }
    }
}
