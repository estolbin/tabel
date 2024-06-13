using System.ComponentModel.DataAnnotations;

namespace web_tabel.Domain
{
    public class Role
    {
        [Key]
        [MaxLength(50)]
        public string Name { get; set; }
        [DataType(DataType.Text), MaxLength(255)]
        public string Description { get; set; }
        [Display(Name = "Согласующий")]
        public bool IsApproving { get; set; } // признак, что роль может согласовывать
        [Display(Name = "Ограничение на редактирование")]
        public bool HasRestriction { get; set; } // признак ограничения на возможное количество дней для редактирования
        [Display(Name = "Количество редактируемых дней (от, до)")]
        public int DaysCount { get; set; } = 3; // количество редактируемых дней

        /// <summary>
        /// Возвращает коллекцию дат, доступных для редактирования
        /// </summary>
        /// <returns>List<DateTime>()</returns>
        public List<DateTime> EditDatesList()
        {
            var result = new List<DateTime>();
            var numDays = (int)(DaysCount / 2);
            for (var i = DateTime.Now.AddDays(-numDays); i <= DateTime.Now.AddDays(numDays); i = i.AddDays(1))
            {
                result.Add(DateTime.Now);
            }
            return result;
        }

    }
}
