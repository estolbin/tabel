using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_table.Web.Controllers
{
    [Controller]
    public class StaffScheduleController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private IEnumerable<Organization> _organizations;

        public StaffScheduleController() 
        {
            _unitOfWork = new UnitOfWork();
        }

        public Task<IActionResult> Index()
        {
            SetViewBagForSelect();

            var list = _unitOfWork.StaffScheduleRepository.GetAll();
            return Task.FromResult<IActionResult>(View(list));
        }

        private async void SetViewBagForSelect()
        {
            if (_organizations == null)
                _organizations = _unitOfWork.OrganizationRepository.GetAll();

            ViewBag.Organizations = _organizations.ToSelectListItem(x => x.Id, x => x.Name, TempData["orgId"] as string[]);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string[] orgId)
        {
            if (orgId.Length == 0)
                return RedirectToAction("Index");

            foreach (var item in orgId)
            {
                var id = new Guid(item);
                var org = _unitOfWork.OrganizationRepository.SingleOrDefault(x => x.Id == id);
                TempData["orgId"] = org.Name;
            }
            return RedirectToAction("Index");
        }
    }
}
