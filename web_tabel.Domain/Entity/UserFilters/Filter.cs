using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_tabel.Domain.UserFilters
{
    public abstract class Filter
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name="Название фильтра")]
        public string Name { get; set; }
        [Display(Name = "Тип фильтра")]
        public string FilterType { get; set; }

    }
}
