using web_tabel.Domain;

namespace web_table.Web.ViewModel
{
    public class FilterItemEditViewModel
    {
        public List<Department> Departments { get; set; }
        public List<Organization> Organizations { get; set; }
        public string FilterType { get; set; }
        public Guid SelectedDepartment { get; set; }
        public Guid SelectedOrganization { get; set; }
    }
}
