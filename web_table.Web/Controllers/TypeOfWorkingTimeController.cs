using Microsoft.AspNetCore.Mvc;
using web_tabel.Services;

namespace web_table.Web.Controllers
{
    public class TypeOfWorkingTimeController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public TypeOfWorkingTimeController()
        {
            _unitOfWork = new UnitOfWork();
        }


        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.TypeOfWorkingTimeRepository.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SetColor(string name, string color)
        {
            name = name ?? "";  
            var type = await _unitOfWork.TypeOfWorkingTimeRepository.SingleOrDefaultAsync(x => x.Name == name);
            type.ColorText = color;
            await _unitOfWork.TypeOfWorkingTimeRepository.UpdateAsync(type);
            await _unitOfWork.SaveAsync();
            return View("Index", await _unitOfWork.TypeOfWorkingTimeRepository.GetAllAsync());
        }
    }
}
