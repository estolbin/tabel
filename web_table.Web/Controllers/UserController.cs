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

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(x => x.Id == id);
            await SetViewBag();
            return View(user);
        }

        private async Task SetViewBag()
        {
            ViewBag.Roles = (await _unitOfWork.RoleRepository.GetAllAsync()).ToSelectListItem(x => x.Name, x => x.Description, null);
            ViewBag.Filters = (await _unitOfWork.FilterRepository.GetAllAsync()).ToSelectListItem(x => x.Id, x => x.Name, null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] AppUser model)
        {
            ModelState.Clear();

            if (String.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("Name", "Не заполнено имя пользователя");
            }
            if(String.IsNullOrEmpty(model.RoleName))
            {
                ModelState.AddModelError("RoleName", "Не выбрана роль");
            }
            if(String.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("Email", "Не заполнен Email");
            }


            if(!ModelState.IsValid)
            {
                await SetViewBag();
                return View(model);
            }
            var existingUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(x => x.Id == model.Id);
            if (existingUser == null)
            {
                return NotFound();
            }
            if(!String.IsNullOrEmpty(model.Password) && existingUser.Password != model.Password)
                existingUser.Password = model.Password;

            if (existingUser.RoleName != model.RoleName)
            {
                existingUser.Role = await _unitOfWork.RoleRepository.SingleOrDefaultAsync(x => x.Name == model.RoleName);
                existingUser.RoleName = model.RoleName;
            }

            if(model.FilterId != existingUser.FilterId)
            {
                existingUser.Filter = await _unitOfWork.FilterRepository.SingleOrDefaultAsync(x => x.Id == model.FilterId);
                existingUser.FilterId = model.FilterId;
            }

            await _unitOfWork.UserRepository.UpdateAsync(existingUser);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
