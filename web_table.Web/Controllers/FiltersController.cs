using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_tabel.Domain.UserFilters;
using web_tabel.Services;
using web_tabel.Services.TimeShiftContext;
using web_table.Web.ViewModel;

namespace web_table.Web.Controllers
{
    public class FiltersController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public FiltersController()
        {
            _unitOfWork= new UnitOfWork();
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.FilterRepository.GetAllAsync());
        }

        //public async Task<IActionResult> Details(int? id)
        //{
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var filter = await _context.Filters
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (filter == null)
            //{
            //    return NotFound();
            //}

            //return View(filter);
        //}

        public async Task<IActionResult> Create()
        {
            var model = new CreateFilterViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFilterViewModel model)
        {
            ModelState.Clear();   
            if(string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("Name", "Необходимо ввести название фильтра");
            }

            if(string.IsNullOrEmpty(model.FilterType))
            {
                ModelState.AddModelError("FilterType", "Необходимо выбрать тип фильтра");
            }

            if(model.FilterType == "Organization" || model.FilterType == "Composite")
            {
                if(model.OrganizationIds == null || !model.OrganizationIds.Any())
                {
                    ModelState.AddModelError("OrganizationIds", "Необходимо выбрать организацию");
                }
            }

            if(model.FilterType == "Department" || model.FilterType == "Composite")
            {
                if(model.DepartmentIds == null || !model.DepartmentIds.Any())
                {
                    ModelState.AddModelError("DepartmentIds", "Необходимо выбрать подразделение ");
                }
            }

            if(!ModelState.IsValid)
            {
                return View(model);
            }

            Filter filter;
            switch(model.FilterType)
            {
                case "Organization":
                    filter = new OrganizationFilter
                    {
                        Name = model.Name,
                        OrganizationIds = model.OrganizationIds
                    };
                    break;
                case "Department":
                    filter = new DepartmentFilter
                    {
                        Name = model.Name,
                        DepartmentIds = model.DepartmentIds
                    };
                    break;
                case "Composite":
                    filter = new CompositeFilter
                    {
                        Name = model.Name,
                        OrganizationIds = model.OrganizationIds,
                        DepartmentIds = model.DepartmentIds
                    };
                    break;
                default:
                    ModelState.AddModelError("", "Неизвестный тип фильтра");
                    return View(model);
            }

            await _unitOfWork.FilterRepository.InsertAsync(filter);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Filters/Edit/5
       // public async Task<IActionResult> Edit(int? id)
        //{
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var filter = await _context.Filters.FindAsync(id);
            //if (filter == null)
            //{
            //    return NotFound();
            //}
            //return View(filter);
        //}

        // POST: Filters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FilterType")] Filter filter)
        //{
           // if (id != filter.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(filter);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!FilterExists(filter.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(filter);
        //}

        // GET: Filters/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var filter = await _context.Filters
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (filter == null)
            //{
            //    return NotFound();
            //}

            //return View(filter);
        //}

        // POST: Filters/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
            //var filter = await _context.Filters.FindAsync(id);
            //if (filter != null)
            //{
            //    _context.Filters.Remove(filter);
            //}

            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        //}

        private async Task<bool> FilterExists(int id)
        {
            return (await _unitOfWork.FilterRepository.SingleOrDefaultAsync(x => x.Id == id) != null);
        }
    }
}
