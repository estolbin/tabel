
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using web_tabel.Domain.UserFilters;

namespace web_tabel.Domain
{
    public class AppUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100)]
        [Display(Name = "Имя пользователя")]
        public string Name { get; set; }
        [MaxLength(100)]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [MaxLength(100)]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public virtual Role Role { get; set; }
        public string RoleName { get; set; }

        public virtual Filter Filter { get; set; }
        public int? FilterId { get; set;  }

    }
}
