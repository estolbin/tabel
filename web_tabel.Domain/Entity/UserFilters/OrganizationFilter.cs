

namespace web_tabel.Domain.UserFilters
{
    public class OrganizationFilter : Filter
    {
        public List<Guid> OrganizationIds { get; set; }
        public virtual List<Organization> Organizations { get; set; }
    }
}
