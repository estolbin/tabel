using System.ComponentModel.DataAnnotations;
using web_table.Web.Services;

namespace web_table.Web.ViewModel
{
    public class EditFilterViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Название фильтра")]
        public string Name { get; set; }
        [Display(Name = "Тип фильтра")]
        public string FilterType { get; set; }
        [Display(Name = "Организация")]
        public List<Guid> OrganizationIds { get; set; }
        [Display(Name = "Подразделение")]
        public List<Guid> DepartmentIds { get; set; }

    }
}
