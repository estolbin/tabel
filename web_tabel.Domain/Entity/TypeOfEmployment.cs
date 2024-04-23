using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_tabel.Domain
{
    /// <summary>
    /// Вид занятости сотрудника (основное место работы, по совместительству)
    /// </summary>
    public class TypeOfEmployment : Entity
    {
        public string Name { get; set; }
    }
}
