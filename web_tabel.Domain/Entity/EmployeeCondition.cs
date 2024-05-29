using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace web_tabel.Domain
{
    /// <summary>
    /// Состояния сотрудников. Общее перечисление, какие могут быть состояния.
    /// </summary>
    public class EmployeeCondition
    {
        public string Name { get; set; }
        [JsonIgnore]
        public virtual TypeOfWorkingTime? TypeOfWorkingTime { get; set; }
    }
}
