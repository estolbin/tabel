using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_table.Web.Controllers
{
    public class TypeOfWorkingTimeRulesController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public TypeOfWorkingTimeRulesController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public async Task<IActionResult> Index()
        {
            var list = await _unitOfWork.TypeOfWorkingTimeRulesRepository.GetAllAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var types = await _unitOfWork.TypeOfWorkingTimeRepository.GetAllAsync();
            ViewBag.Types = types.ToSelectListItem(x => x.Name, x => x.Description, null);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TypeOfWorkingTimeRules type)
        {
            type.SourceName = type.SourceName ?? "";
            type.TargetName = type.TargetName ?? "";
            type.Source = await _unitOfWork.TypeOfWorkingTimeRepository.SingleOrDefaultAsync(x => x.Name == type.SourceName);
            type.Target = await _unitOfWork.TypeOfWorkingTimeRepository.SingleOrDefaultAsync(x => x.Name == type.TargetName);

            await _unitOfWork.TypeOfWorkingTimeRulesRepository.InsertAsync(type);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private async void SetViewBag()
        {
            if (ViewBag.Targets != null) return;
            var list = await _unitOfWork.TypeOfWorkingTimeRepository.GetAllAsync();
            ViewBag.Targets = list.ToSelectListItem(x => x.Name, x => x.Description, null);

        }

        public async Task<IActionResult> Rules(string name)
        {
            name = name ?? "";
            var source = await _unitOfWork.TypeOfWorkingTimeRepository.SingleOrDefaultAsync(x => x.Name == name);
            if (source == null) return NotFound();
            SetViewBag();
            var selected = await _unitOfWork.TypeOfWorkingTimeRulesRepository.GetAsync(x => x.Source == source);
            if (selected != null)
                ViewBag.Selected = selected.Select(x => x.Target.Name).ToArray();
            else
                ViewBag.Selected = null;

            return View(source);
        }

        public ActionResult DisplayRule()
        {
            SetViewBag();
            return PartialView("_RulesSetDiv");
        }

        [HttpPost]
        public async Task<IActionResult> Save(string source, string[] targets, string[] deletedName = null)
        {
            source = source ?? "";
            List<TypeOfWorkingTimeRules> res = new List<TypeOfWorkingTimeRules>();
            var sourceType = await _unitOfWork.TypeOfWorkingTimeRepository.SingleOrDefaultAsync(x => x.Name == source);

            if(deletedName != null)
            {
                for (int i = 0; i < deletedName.Length; i++)
                {
                    var targetType = await _unitOfWork.TypeOfWorkingTimeRepository.SingleOrDefaultAsync(x => x.Name == deletedName[i]);
                    var todelete = await _unitOfWork.TypeOfWorkingTimeRulesRepository.SingleOrDefaultAsync(x => x.Source == sourceType && x.Target == targetType);
                    await _unitOfWork.TypeOfWorkingTimeRulesRepository.DeleteAsync(todelete);
                    await _unitOfWork.SaveAsync();
                }
            }

            for (int i = 0; i < targets.Length; i++)
            {
                var targetType = await _unitOfWork.TypeOfWorkingTimeRepository.SingleOrDefaultAsync(x => x.Name == targets[i]);

                var exist = await _unitOfWork.TypeOfWorkingTimeRulesRepository.SingleOrDefaultAsync(x => x.Source == sourceType && x.Target == targetType);
                if (exist != null) continue;

                res.Add(new TypeOfWorkingTimeRules { Source = sourceType, Target = targetType, SourceName = source, TargetName = targets[i] });
            }
            await _unitOfWork.TypeOfWorkingTimeRulesRepository.AddRangeAsync(res);
            await _unitOfWork.SaveAsync();

            
            return Json(new {redirectToUrl = Url.Action("Index", "TypeOfWorkingTimeRules")});
        }
    }
}
