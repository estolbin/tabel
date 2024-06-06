using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace web_tabel.Domain
{
    /// <summary>
    /// Состояния сотрудника
    /// </summary>
    public class EmployeeState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        
        [ForeignKey("EmployeeId")]
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [ForeignKey("ConditionName")]
        public string ConditionName { get; set; }
        public virtual EmployeeCondition Condition { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
