using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_tabel.Domain
{
    public class TypeOfWorkingTimeRules
    {
        [MaxLength(3)]
        [Display(Name = "Основное значение")]
        public string SourceName { get; set; }
        public virtual TypeOfWorkingTime Source { get; set; }

        [MaxLength(3)]
        [Display(Name = "Возможное значение")]
        public string TargetName { get; set; }
        public virtual TypeOfWorkingTime Target { get; set; }
    }
}
