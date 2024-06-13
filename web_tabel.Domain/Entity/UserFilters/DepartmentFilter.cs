

namespace web_tabel.Domain.UserFilters
{
    public class DepartmentFilter : Filter
    {
        public List<Guid> DepartmentIds { get; set; }
        public virtual List<Department> Departemnts { get; set; }
    }
}
