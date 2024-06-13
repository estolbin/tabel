using System.ComponentModel.DataAnnotations;
using web_table.Web.Services;

namespace web_table.Web.ViewModel
{
    public class CreateFilterViewModel
    {
        public string Name { get; set; }
        
        public string FilterType { get; set; }
        
        //[ValidateListForFilterType("Organization", ErrorMessage="Please select at least one organization")]
        
        public List<Guid> OrganizationIds { get; set; }
        
        //[ValidateListForFilterType("Department", ErrorMessage = "Please select at least one department")]
        
        public List<Guid> DepartmentIds { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (FilterType == "Organization" || FilterType == "Composite")
        //    {
        //        if (OrganizationIds == null || !OrganizationIds.Any())
        //        {
        //            yield return new ValidationResult("Organization IDs are required for Organization or Composite filters.", new[] { nameof(OrganizationIds) });
        //        }
        //    }

        //    if (FilterType == "Department" || FilterType == "Composite")
        //    {
        //        if (DepartmentIds == null || !DepartmentIds.Any())
        //        {
        //            yield return new ValidationResult("Department IDs are required for Department or Composite filters.", new[] { nameof(DepartmentIds) });
        //        }
        //    }
        //}

    }
}
