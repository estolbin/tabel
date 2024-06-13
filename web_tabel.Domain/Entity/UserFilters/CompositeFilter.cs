

namespace web_tabel.Domain.UserFilters
{
    public class CompositeFilter : Filter
    {
        public virtual List<Organization> Organizations { get; set; }
        public List<Guid> OrganizationIds { get; set; }

        public virtual List<Department> Departments { get; set; }
        public List<Guid> DepartmentIds { get; set; }
    }
}
