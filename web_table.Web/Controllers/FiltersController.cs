using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_tabel.Domain;
using web_tabel.Domain.UserFilters;
using web_tabel.Services;
using web_tabel.Services.TimeShiftContext;
using web_table.Web.ViewModel;

namespace web_table.Web.Controllers
{
    public class FiltersController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private IEnumerable<Department> _departments;
        private IEnumerable<Organization> _organizations;

        public FiltersController()
        {
            _unitOfWork= new UnitOfWork();
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.FilterRepository.GetAllAsync());
        }

        public async Task<IActionResult> Create()
        {
            var model = new CreateFilterViewModel();
            await SetViewBag();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(string filterType, string[] departments, string[] organizations, string Name)
        {

            var dep =  _unitOfWork.DepartmentRepository.Get(x => departments.Contains(x.Id.ToString()));
            var org =  _unitOfWork.OrganizationRepository.Get(x => organizations.Contains(x.Id.ToString()));

            Filter filter = null;
            switch(filterType)
            {
                case "Organization":
                    filter = new OrganizationFilter
                    {
                        Name = Name,
                        FilterType = filterType,
                        Organizations = org.ToList(),
                        OrganizationIds = org.Select(x => x.Id).ToList(),
                    };
                    break;
                case "Department":
                    filter = new DepartmentFilter
                    {
                        Name = Name,
                        FilterType = filterType,
                        Departemnts = dep.ToList(),
                        DepartmentIds = dep.Select(x => x.Id).ToList(),

                    };
                    break;
                case "Composite":
                    filter = new CompositeFilter
                    {
                        Name = Name,
                        FilterType = filterType,
                        Organizations = org.ToList(),
                        Departments = dep.ToList(),
                        OrganizationIds = org.Select(x => x.Id).ToList(),
                        DepartmentIds = dep.Select(x => x.Id).ToList()
                    };
                    break;
            }

            _unitOfWork.FilterRepository.Insert(filter!);
            _unitOfWork.Save();
            //turn RedirectToAction(nameof(Index));
            return Json(new { redirectToUrl = Url.Action("Index", "Filters") });

        }


        public async Task<IActionResult> Edit(int? id)
        {
            await SetViewBag();
            if (id == null) return NotFound();

            var filter = await _unitOfWork.FilterRepository.SingleOrDefaultAsync(x => x.Id == id);
            if (filter == null) return NotFound();

            GetOrganizationFilter(filter, out List<Guid> organizationFilter);
            GetDepartmentsFilter(filter, out List<Guid> departmentFilter );

            var editFilter = new EditFilterViewModel
            {
                Id = filter.Id,
                Name = filter.Name,
                FilterType = filter.FilterType,
                OrganizationIds = organizationFilter,
                DepartmentIds = departmentFilter
            };
            return View(editFilter);
        }

        private async Task SetViewBag()
        {
            if (_departments == null)
                _departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            if (_organizations == null)
                _organizations = await _unitOfWork.OrganizationRepository.GetAllAsync();
            ViewBag.Departments = _departments;
            ViewBag.Organizations = _organizations;
        }

        public async Task<IActionResult> DisplayItem(string FilterType)
        {
            await SetViewBag();

            var model = new FilterItemEditViewModel
            {
                FilterType = FilterType,
                Organizations = _organizations.ToList(),
                Departments = _departments.ToList(),
            };

            return PartialView("_FilterItem", model);
        }

        private void GetDepartmentsFilter(Filter filter, out List<Guid> departmentFilter)
        {
            departmentFilter = new List<Guid>();
            if(filter is DepartmentFilter)
            {
                departmentFilter = (filter as DepartmentFilter).DepartmentIds;
            }
            if(filter is CompositeFilter)
            {
                departmentFilter = (filter as CompositeFilter).DepartmentIds;
            }
        }

        private void GetOrganizationFilter(Filter filter, out List<Guid> organizationFilter)
        {
            organizationFilter = new List<Guid>();
            if(filter is OrganizationFilter)
            {
                organizationFilter = (filter as OrganizationFilter).OrganizationIds;
            }
            if(filter is CompositeFilter)
            {
                organizationFilter = (filter as CompositeFilter).OrganizationIds;
            }
        }

        private async Task<bool> FilterExists(int id)
        {
            return (await _unitOfWork.FilterRepository.SingleOrDefaultAsync(x => x.Id == id) != null);
        }

        [HttpPost]
        public IActionResult Save(int filterId, 
            string filterType, string[] departments, 
            string[] organizations)
        {

                
            Filter filter;
            var existedFilter = _unitOfWork.FilterRepository.SingleOrDefault(x => x.Id == filterId);

            if(existedFilter == null) { }

            if (departments.Length > 0 && (filterType == "Department" || filterType == "Composite"))
            {
                var deps = _unitOfWork.DepartmentRepository.Get(x => departments.Contains(x.Id.ToString()));
                if(filterType == "Department")
                {
                    (existedFilter as DepartmentFilter).Departemnts = deps.ToList();
                    (existedFilter as DepartmentFilter).DepartmentIds = deps.Select(x => x.Id).ToList();
                }
                else
                {
                    (existedFilter as CompositeFilter).Departments = deps.ToList();
                    (existedFilter as CompositeFilter).DepartmentIds = deps.Select(x => x.Id).ToList();
                }
            }

            if(organizations.Length > 0 && (filterType == "Organization" || filterType == "Composite"))
            {
                var orgs = _unitOfWork.OrganizationRepository.Get(x => organizations.Contains(x.Id.ToString())).ToList();
                if (filterType == "Organization")
                {

                    (existedFilter as OrganizationFilter).Organizations = orgs;
                    (existedFilter as OrganizationFilter).OrganizationIds = orgs.Select(x => x.Id).ToList();
                }
                else
                {
                    (existedFilter as CompositeFilter).Organizations = orgs;
                    (existedFilter as CompositeFilter).OrganizationIds = orgs.Select(x => x.Id).ToList();
                }
            }
            _unitOfWork.FilterRepository.Update(existedFilter);
            _unitOfWork.Save();


            return Json(new { redirectToUrl = Url.Action("Index", "Filters") });
        }
    }
}
